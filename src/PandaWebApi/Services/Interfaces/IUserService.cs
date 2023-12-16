using Pandatech.Crypto;
using PandaTech.IEnumerableFilters.Dto;
using PandaTech.ServiceResponse;
using PandaWebApi.DTOs;
using PandaWebApi.DTOs.Authentication;
using PandaWebApi.DTOs.User;
using PandaWebApi.Helpers;
using PandaWebApi.Models;
using ResponseCrafter.Dtos;
using IdentifyUserDto = PandaWebApi.DTOs.Authentication.IdentifyUserDto;

namespace PandaWebApi.Services.Interfaces;

public interface IUserService
{
    public Task CreateUserAsync(CreateUserDto createUserDto);
    public Task UpdateUserAsync(UpdateUserDto updateUserDto);
    public Task UpdatePasswordAsync(UpdatePasswordDto updatePasswordDto);
    public Task UpdateUserStatusAsync(UpdateUserStatusDto updateUserStatusDto);
    public Task DeleteUsersAsync(List<long> ids);
    public Task<PagedResponse<GetUserDto>> GetUsersAsync(int page, int pageSize, GetDataRequest request);
    public Task<List<FilterInfo>> GetUserFiltersAsync();
    public Task<DistinctColumnValuesResult> UserColumnValuesAsync(string columnName, string filterString, int page, int pageSize);
    public Task<object?> UserAggregateAsync(string columnName, string filterString, AggregateType aggregate);
    public Task<List<GetUserDto>> ExportUsersAsync(GetDataRequest request);
    public void SetUserContext(Token token, ContextUser contextUser);
}