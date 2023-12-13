using PandaTech.IEnumerableFilters.Dto;
using PandaTech.ServiceResponse;
using PandaWebApi.DTOs.Authentication;
using PandaWebApi.DTOs.UserManagement;
using ResponseCrafter.Dtos;
using IdentifyUserDto = PandaWebApi.DTOs.Authentication.IdentifyUserDto;

namespace PandaWebApi.Services.Interfaces;

public interface IUserManagementService
{
    public Task<LoginResponseDto> LoginAsync(LoginDto loginDto, HttpContext httpContext);

    public Task<List<GetUserDto>> GetAllUsersForExportAsync(GetDataRequest request);
    public Task UpdatePasswordAsync(ChangePasswordDto changePasswordDto);
    public Task CreateUserAsync(AddUserDto addUserDto);
    public Task<PagedResponse<GetUserDto>> GetUsersAsync(int page, int pageSize, GetDataRequest request);
    public Task UpdateUserAsync(UpdateUserDto updateUserDto);
    public Task DeleteUsersAsync(List<long> ids);
    public IdentifyUserDto IdentifyUser();
    public Task UpdateUserStatusAsync(UpdateUserStatusDto updateUserStatusDto);
    public Task<List<FilterInfo>> GetUserFiltersAsync();
    public Task<DistinctColumnValuesResult> UserColumnValuesAsync(string columnName, string filterString, int page, int pageSize);
    public Task ForcePasswordChangeAsync(string password);
    public Task ChangeOwnPasswordAsync(ChangeOwnPasswordDto changeOwnPasswordDto);
}