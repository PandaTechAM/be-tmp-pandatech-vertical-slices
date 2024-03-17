using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using PandaTech.IEnumerableFilters;
using PandaTech.IEnumerableFilters.Dto;
using PandaTech.IEnumerableFilters.Enums;
using PandaTech.IEnumerableFilters.Extensions;
using Pandatech.VerticalSlices.Domain.EntityFilters;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.DTOs.User;
using Pandatech.VerticalSlices.Infrastructure.Contexts;
using ResponseCrafter.Dtos;
using ResponseCrafter.StandardHttpExceptions;

namespace Pandatech.VerticalSlices.Services.Implementations;

public class UserService(
  Argon2Id argon2Id,
  PostgresContext context)
{

  public async Task UpdateUserAsync(UpdateUserDto updateUserDto)
  {
    if (updateUserDto.UserRole == UserRole.SuperAdmin)
      throw new ForbiddenException("superadmin_cannot_be_updated");

    var checkIfUsernameExists =
      await context.Users.FirstOrDefaultAsync(x => x.Username == updateUserDto.Username.ToLower());
    if (checkIfUsernameExists != null && checkIfUsernameExists.Id != updateUserDto.Id)
      throw new BadRequestException("username_already_exists");

    var user = await context.Users.FirstOrDefaultAsync(x => x.Id == updateUserDto.Id);

    if (user == null)
      throw new NotFoundException("user_not_found");

    user.FullName = updateUserDto.FullName;
    user.Role = updateUserDto.UserRole;
    user.Username = updateUserDto.Username.ToLower();
    user.Comment = updateUserDto.Comment;

    await context.SaveChangesAsync();
  }

  public async Task UpdatePasswordAsync(UpdatePasswordDto updatePasswordDto)
  {
    var isValidPassword = Password.Validate(updatePasswordDto.NewPassword, 8, true, true, true, false);

    if (!isValidPassword)
      throw new BadRequestException(
        "password_should_contain_at_least_8_characters_one_lowercase_one_uppercase_one_digit");

    var userToUpdate = await context.Users.FirstOrDefaultAsync(x => x.Id == updatePasswordDto.Id);
    if (userToUpdate == null)
      throw new NotFoundException();

    if (userToUpdate.Role == UserRole.SuperAdmin)
      throw new ForbiddenException("superadmin_password_cannot_be_changed");

    userToUpdate.PasswordHash = argon2Id.HashPassword(updatePasswordDto.NewPassword);
    await context.SaveChangesAsync();

    //await userTokenService.RevokeAllTokensAsync(userToUpdate.Id);
  }

  public async Task UpdateUserStatusAsync(UpdateUserStatusDto updateUserStatusDto)
  {
    var userToUpdate = await context.Users.FirstOrDefaultAsync(x => x.Id == updateUserStatusDto.Id);

    if (userToUpdate is null)
      throw new NotFoundException();

    if (userToUpdate.Role == UserRole.SuperAdmin)
      throw new ForbiddenException("superadmin_cannot_be_updated");

    userToUpdate.Status = updateUserStatusDto.Status;
    await context.SaveChangesAsync();
  }

  public async Task DeleteUsersAsync(List<long> ids)
  {
    var users = await context.Users
      .Where(x => ids.Contains(x.Id)).ToListAsync();

    if (users.Count == 0)
      throw new NotFoundException("users_not_found");

    if (users.Exists(user => user.Role == UserRole.SuperAdmin))
    {
      throw new ForbiddenException("superadmin_cannot_be_deleted");
    }


    foreach (var user in users)
    {
      user.Status = UserStatus.Deleted;
    }

    await context.SaveChangesAsync();
  }

  public async Task<PagedResponse<GetUserDto>> GetUsersAsync(int page, int pageSize, GetDataRequest request)
  {
    if (page < 1)
      throw new BadRequestException("page must be positive number");
    if (pageSize < 1)
      throw new BadRequestException("pageSize must be positive number");

    var usersQuery = context.Users
      .Where(s => s.Status != UserStatus.Deleted && s.Role != UserRole.SuperAdmin)
      .AsQueryable();

    usersQuery = usersQuery
      .Where(x => x.Status != UserStatus.Deleted)
      .OrderByDescending(x => x.FullName)
      .ApplyFilters(request.Filters)
      .ApplyOrdering(request.Order);

    var users = (await usersQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync())
      .Select(u => new GetUserDto
      {
        Id = u.Id,
        Username = u.Username,
        FullName = u.FullName,
        Role = new RolesSelect { Id = u.Role, Name = u.Role, },
        CreatedAt = u.CreatedAt,
        Status = u.Status,
        Comment = u.Comment
      }).ToList();

    var totalCount = await usersQuery.CountAsync();

    var response = new PagedResponse<GetUserDto>(users, page, pageSize, totalCount);

    return response;
  }

  public async Task<List<FilterInfo>> GetUserFiltersAsync()
  {
    return await Task.FromResult(FilterExtenders.GetFilters(nameof(UserEntityFilter)));
  }

  public async Task<DistinctColumnValuesResult> UserColumnValuesAsync(string columnName, string filterString,
    int page, int pageSize)
  {
    var q = context.Users
      .Where(x => x.Status != UserStatus.Deleted && x.Role != UserRole.SuperAdmin)
      .AsQueryable();

    return await q.DistinctColumnValuesAsync(GetDataRequest.FromString(filterString).Filters, columnName, pageSize,
      page);
  }

  public async Task<object?> UserAggregateAsync(string columnName, string filterString, AggregateType aggregate)
  {
    var request = GetDataRequest.FromString(filterString);

    return await context.Users
      .ApplyFilters(request.Filters)
      .AggregateAsync(columnName, aggregate);
  }

  public async Task<List<GetUserDto>> ExportUsersAsync(GetDataRequest request)
  {
    var usersQuery = context.Users
      .Where(s => s.Status != UserStatus.Deleted && s.Role != UserRole.SuperAdmin)
      .AsQueryable();

    usersQuery = usersQuery
      .Where(x => x.Status != UserStatus.Deleted)
      .OrderBy(x => x.FullName)
      .ApplyFilters(request.Filters)
      .ApplyOrdering(request.Order);

    var users = await usersQuery
      .Select(u => new GetUserDto
      {
        Id = u.Id,
        Username = u.Username,
        FullName = u.FullName,
        Role = new RolesSelect { Id = u.Role, Name = u.Role, },
        CreatedAt = u.CreatedAt,
        Status = u.Status,
        Comment = u.Comment
      }).ToListAsync();

    var response = new List<GetUserDto>(users);

    return response;
  }
}
