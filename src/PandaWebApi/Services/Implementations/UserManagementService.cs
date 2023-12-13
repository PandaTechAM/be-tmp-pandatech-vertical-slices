using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using PandaTech.IEnumerableFilters;
using PandaTech.IEnumerableFilters.Dto;
using PandaWebApi.Contexts;
using PandaWebApi.DTOs.Authentication;
using PandaWebApi.DTOs.UserManagement;
using PandaWebApi.Enums;
using PandaWebApi.Helpers;
using PandaWebApi.Models;
using PandaWebApi.Services.Interfaces;
using ResponseCrafter.Dtos;
using ResponseCrafter.StandardHttpExceptions;
using IdentifyUserDto = PandaWebApi.DTOs.Authentication.IdentifyUserDto;

namespace PandaWebApi.Services.Implementations;

public class UserManagementService : IUserManagementService
{
    private readonly PostgresContext _context;
    private readonly Argon2Id _argon2Id;
    private readonly RequestContextDataProvider _requestContextDataProvider;
    private readonly string _domain;
    private readonly IConfiguration _configuration;

    public UserManagementService(Argon2Id argon2Id, PostgresContext context, RequestContextDataProvider requestContextDataProvider, IConfiguration configuration)
    {
        _argon2Id = argon2Id;
        _context = context;
        _requestContextDataProvider = requestContextDataProvider;
        _configuration = configuration;
        _domain = configuration["Security:CookieDomain"]!;

    }
    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto, HttpContext httpContext)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username.ToLower());
        var newHistory = new UserAuthenticationHistory();
        if (user == null || user.Status == Statuses.Deleted)
        {
            throw new BadRequestException("Invalid username or password.");
        }

        if (user.Status == Statuses.Disabled)
        {
            newHistory.UserId = user.Id;
            newHistory.AuthenticationDate = DateTime.UtcNow;
            newHistory.IsAuthenticated = false;

            await _context.UserAuthenticationHistory.AddAsync(newHistory);
            await _context.SaveChangesAsync();

            throw new BadRequestException("Invalid username or password.");
        }

        var history = await _context.UserAuthenticationHistory
            .Where(u => u.UserId == user.Id)
            .OrderByDescending(u => u.AuthenticationDate)
            .Take(3).ToListAsync();

        if (history.Count == 3 && history.TrueForAll(h => !h.IsAuthenticated) &&
            history.Exists(h => h.AuthenticationDate > DateTime.UtcNow.AddSeconds(-30)))
        {
            throw new BadRequestException("Too many failed login attempts. Please try again after 30 seconds.");
        }

        if (!_argon2Id.VerifyHash(loginDto.Password, user.PasswordHash))
        {
            newHistory.UserId = user.Id;
            newHistory.AuthenticationDate = DateTime.UtcNow;
            newHistory.IsAuthenticated = false;

            await _context.UserAuthenticationHistory.AddAsync(newHistory);
            await _context.SaveChangesAsync();

            throw new BadRequestException("Invalid username or password.");
        }

        int expirationMinutesInt = 30;
        var expirationMinutesString = _configuration["Security:TokenExpirationMinutes"];
        if (Int32.TryParse(expirationMinutesString, out int expirationMinutes))
        {
            expirationMinutesInt = expirationMinutes;
        }
        
        var token = new Token
        {
            TokenString = Guid.NewGuid(),
            ExpirationDate = DateTime.UtcNow.AddMinutes(expirationMinutesInt),
            UserId = user.Id,
            CreationDate = DateTime.UtcNow
        };
        await _context.Tokens.AddAsync(token);
        await _context.SaveChangesAsync();


        var successHistory = new UserAuthenticationHistory()
        {
            UserId = user.Id,
            AuthenticationDate = DateTime.UtcNow,
            IsAuthenticated = true
        };
        await _context.UserAuthenticationHistory.AddAsync(successHistory);
        await _context.SaveChangesAsync();

        
        httpContext.Response.Cookies.Append(
            "token", token.TokenString.ToString(),
            new CookieOptions
            {
                Expires = token.ExpirationDate,
                HttpOnly = true,
                Secure = true,
                Domain = _domain
            }
        );

        var loginResponse = new LoginResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            TokenExpirationDate = token.ExpirationDate,
            Role = user.Role,
            ForcePasswordChange = user.ForcePasswordChange
        };
        return loginResponse;
    }

    public async Task<List<GetUserDto>> GetAllUsersForExportAsync(GetDataRequest request)
    {
        var usersQuery = _context.Users
            .Where(s => s.Status != Statuses.Deleted && s.Role != Roles.SuperAdmin)
            .AsQueryable();

        usersQuery = usersQuery
            .Where(x => x.Status != Statuses.Deleted)
            .OrderByDescending(x => x.CreationDate)
            .ApplyFilters<User, GetUserDto>(request.Filters)
            .ApplyOrdering<User, GetUserDto>(request.Order);

        var users = (await usersQuery
            .Select(u => new GetUserDto
            {
                Id = u.Id,
                Username = u.Username,
                FullName = u.FullName,
                Role = new RolesSelect()
                {
                    Id = u.Role,
                    Name = u.Role,
                },
                CreationDate = u.CreationDate,
                Status = u.Status,
                Comment = u.Comment
            }).ToListAsync());

        var response = new List<GetUserDto>(users);

        return response;
    }

    public async Task UpdatePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        var user = _requestContextDataProvider.User;
        var userToUpdate = await _context.Users.FirstOrDefaultAsync(x => x.Id == changePasswordDto.Id);
        if (userToUpdate == null)
            throw new BadRequestException("user does not exist");

        if (userToUpdate.Role == Roles.SuperAdmin && user.Role != Roles.SuperAdmin)
            throw new ForbiddenException("you cant change superAdmins password");

        if (!Password.Validate(changePasswordDto.NewPassword, 8, false, false, false, false))
            throw new BadRequestException("invalid password"); //todo change
        
        userToUpdate.PasswordHash = _argon2Id.HashPassword(changePasswordDto.NewPassword);
        throw new NotImplementedException();
    }

    public async Task CreateUserAsync(AddUserDto addUserDto)
    {
        if (!Password.Validate(addUserDto.Password, 8, false, false, false, false))
            throw new BadRequestException("invalid password"); //todo change
        
        if (addUserDto.Role == Roles.SuperAdmin)
            throw new ForbiddenException("you cant create second superAdmin");
        
        var checkIfUsernameExists = _context.Users.FirstOrDefaultAsync(x => x.Username == addUserDto.Username);
        if (checkIfUsernameExists != null)
            throw new BadRequestException("username already exists");
        
        var user = new User()
        {
            FullName = addUserDto.FullName,
            CreationDate = DateTime.UtcNow,
            ForcePasswordChange = true,
            Role = addUserDto.Role,
            Username = addUserDto.Username,
            Status = Statuses.Active,
            Comment = addUserDto.Comment,
            PasswordHash = _argon2Id.HashPassword(addUserDto.Password)
        };

        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResponse<GetUserDto>> GetUsersAsync(int page, int pageSize, GetDataRequest request)
    {
        if (page < 1)
            throw new BadRequestException("page must be positive number");
        if (pageSize < 1)
            throw new BadRequestException("pageSize must be positive number");

        var usersQuery = _context.Users
            .Where(s => s.Status != Statuses.Deleted && s.Role != Roles.SuperAdmin)
            .AsQueryable();

        usersQuery = usersQuery
            .Where(x => x.Status != Statuses.Deleted)
            .OrderByDescending(x => x.CreationDate)
            .ApplyFilters<User, GetUserDto>(request.Filters)
            .ApplyOrdering<User, GetUserDto>(request.Order);

        var users = (await usersQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync())
            .Select(u => new GetUserDto
            {
                Id = u.Id,
                Username = u.Username,
                FullName = u.FullName,
                Role = new RolesSelect()
                {
                    Id = u.Role,
                    Name = u.Role,
                },
                CreationDate = u.CreationDate,
                Status = u.Status,
                Comment = u.Comment
            }).ToList();

        var totalCount = await usersQuery.CountAsync();

        var response = new PagedResponse<GetUserDto>(users, page, pageSize, totalCount);

        return response;
    }

    public async Task UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        if (updateUserDto.Role == Roles.SuperAdmin)
            throw new ForbiddenException("we can't have second superAdmin");

        var checkIfUsernameExists = await _context.Users.FirstOrDefaultAsync(x => x.Username == updateUserDto.Username);
        if (checkIfUsernameExists != null && checkIfUsernameExists.Id != updateUserDto.Id)
            throw new BadRequestException("username already exists");

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == updateUserDto.Id);

        if (user == null)
            throw new NotFoundException("user not found");

        user.FullName = updateUserDto.FullName;
        user.Role = updateUserDto.Role;
        user.Username = updateUserDto.Username;
        user.Comment = updateUserDto.Comment;
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUsersAsync(List<long> ids)
    {
        var superAdmin = await _context.Users.FirstOrDefaultAsync(x => x.Role == Roles.SuperAdmin);

        if (ids.Contains(superAdmin!.Id))
            throw new ForbiddenException("can't delete superAdmin");

        var usersToDelete = ids.Select(x => new User()
        {
            Id = x
        });
        _context.RemoveRange(usersToDelete);
        await _context.SaveChangesAsync();
    }

    public IdentifyUserDto IdentifyUser()
    {
        var user = _requestContextDataProvider.User;
        return user;
    }

    public async Task UpdateUserStatusAsync(UpdateUserStatusDto updateUserStatusDto)
    {
        var userToUpdate = await _context.Users.FirstOrDefaultAsync(x => x.Id == updateUserStatusDto.Id);

        if (userToUpdate is null)
            throw new NotFoundException("user not found");
        
        if (userToUpdate.Role == Roles.SuperAdmin)
            throw new ForbiddenException("you can't change superAdmin status");

        userToUpdate.Status = updateUserStatusDto.Status;
        await _context.SaveChangesAsync();
    }

    public async Task<List<FilterInfo>> GetUserFiltersAsync()
    {
        return await Task.FromResult(EnumerableExtendersV3.GetFilters(nameof(GetUserDto)));
    }

    public async Task<DistinctColumnValuesResult> UserColumnValuesAsync(string columnName, string filterString, int page, int pageSize)
    {
        var q = _context.Users
            .Where(x => x.Status != Statuses.Deleted)
            .AsQueryable();

        return await q
            .DistinctColumnValuesAsync<User, GetUserDto>(
                GetDataRequest.FromString(filterString).Filters,
                columnName,
                pageSize,
                page);
    }
    
    public async Task LogoutAllAsync(long userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            throw new NotFoundException("user does not exist");

        var tokens = await _context.Tokens.Where(t => t.UserId == userId).ToListAsync();

        if (tokens.Count == 0)
            return;

        foreach (var userToken in tokens.Where(t => t.ExpirationDate > DateTime.UtcNow))
        {
            userToken.ExpirationDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }
    public async Task ForcePasswordChangeAsync(string password)
    {
        var token = _requestContextDataProvider.User.Token;
        var tokenToVerify = await _context.Tokens.Include(u => u.User)
            .FirstOrDefaultAsync(t => t.TokenString == token);

        if (tokenToVerify == null || tokenToVerify.ExpirationDate < DateTime.UtcNow)
            throw new BadRequestException("Invalid userToken.");

        if (tokenToVerify.User.Status != Statuses.Active)
            throw new BadRequestException("User is not active");

        if (!tokenToVerify.User.ForcePasswordChange)
            throw new BadRequestException("Cannot use this API if password is not forced to changed");

        var user = tokenToVerify.User;

        user.PasswordHash = _argon2Id.HashPassword(password);
        user.ForcePasswordChange = false;
        await _context.SaveChangesAsync();

        await LogoutAllExceptCurrentSessionAsync(user.Id, token);
    }
    
    public async Task ChangeOwnPasswordAsync(ChangeOwnPasswordDto changeOwnPasswordDto) 
    {
        var userInfo = _requestContextDataProvider.User;
        var userId = userInfo.Id;

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            throw new NotFoundException("Id is not found");

        if (user.Role == Roles.SuperAdmin)
            throw new ForbiddenException("Cannot do this action");

        if (!_argon2Id.VerifyHash(changeOwnPasswordDto.OldPassword, user.PasswordHash))
            throw new BadRequestException("Wrong credentials");

        user.PasswordHash = _argon2Id.HashPassword(changeOwnPasswordDto.NewPassword);
        await _context.SaveChangesAsync();

        await LogoutAllExceptCurrentSessionAsync(user.Id, userInfo.Token);
    }
    
    private async Task LogoutAllExceptCurrentSessionAsync(long userId, Guid token)
    {
        var tokens = await _context.Tokens
            .Where(t => t.UserId == userId && t.TokenString != token).ToListAsync();

        foreach (var expiredUserToken in tokens.Where(t => t.ExpirationDate > DateTime.UtcNow))
        {
            expiredUserToken.ExpirationDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }
}