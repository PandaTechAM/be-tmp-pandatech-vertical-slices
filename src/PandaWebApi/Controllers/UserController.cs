using Microsoft.AspNetCore.Mvc;
using PandaFileExporter;
using PandaTech.IEnumerableFilters.Dto;
using PandaTech.ServiceResponse;
using PandaWebApi.Attributes;
using PandaWebApi.DTOs.User;
using PandaWebApi.Enums;
using PandaWebApi.Filters;
using PandaWebApi.Services.Interfaces;

namespace PandaWebApi.Controllers;

[ApiController]
[Route("api/v1")]
[Produces("application/json")]
[Authorize(Roles.User)]
public class UserController(IUserService service) : Controller
{
    [Authorize(Roles.Admin)]
    [HttpPost("user")]
    public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
    {
        await service.CreateUserAsync(createUserDto);
        return Ok();
    }


    [Authorize(Roles.Admin)]
    [HttpPatch("user")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
    {
        await service.UpdateUserAsync(updateUserDto);
        return Ok();
    }

    [Authorize(Roles.Admin)]
    [HttpPatch("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordDto updatePasswordDto)
    {
        await service.UpdatePasswordAsync(updatePasswordDto);
        return Ok();
    }

    [Authorize(Roles.Admin)]
    [HttpPatch("user-status")]
    public async Task<IActionResult> UpdateUserStatus([FromBody] UpdateUserStatusDto updateUserStatusDto)
    {
        await service.UpdateUserStatusAsync(updateUserStatusDto);
        return Ok();
    }
    
    [Authorize(Roles.Admin)]
    [HttpDelete("users")]
    public async Task<IActionResult> DeleteUsers(List<long> ids)
    {
        await service.DeleteUsersAsync(ids);
        return Ok();
    }

    [Authorize(Roles.Admin)]
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(int page, int pageSize, [FromQuery] string dataRequest)
    {
        var request = GetDataRequest.FromString(dataRequest);

        var data = await service.GetUsersAsync(page, pageSize, request);

        return Ok(data);
    }


    [HttpGet("user-filters")]
    [Authorize(Roles.Admin)]
    public async Task<IActionResult> GetUserFilters()
    {
        var result = await service.GetUserFiltersAsync();
        var serviceResponse = new List<FilterInfo>(result);
        return Ok(serviceResponse);
    }

    [HttpGet("user-column-values")]
    [Authorize(Roles.Admin)]
    public async Task<IActionResult> ColumnValues(string columnName, string filterString,
        int page, int pageSize)
    {
        var output = await service.UserColumnValuesAsync(columnName, filterString, page, pageSize);

        var responseData = new ResponseDataPaged<object>
        {
            Data = output.Values,
            TotalCount = output.TotalCount,
            Page = page,
            PageSize = pageSize
        };
        return Ok(responseData);
    }

    [HttpGet("user-aggregate")]
    public async Task<IActionResult> Aggregate(string columnName, string filterString, AggregateType aggregate)
    {
        var output = await service.UserAggregateAsync(columnName, filterString, aggregate);
        return Ok(output);
    }

    [Authorize(Roles.Admin)]
    [HttpGet("users-export")]
    public async Task<IActionResult> ExportUsers([FromQuery] string dataRequest,
        [FromQuery] ExportType exportType)
    {
        string fileExtenstion;
        byte[] exportData;
        string mimeType;

        var request = GetDataRequest.FromString(dataRequest);

        var users = await service.ExportUsersAsync(request);

        switch (exportType)
        {
            case ExportType.CSV:
                fileExtenstion = "csv";
                mimeType = MimeTypes.CSV;
                exportData = HelperMethods.Export(users, ExportType.CSV);
                break;

            case ExportType.PDF:
                fileExtenstion = "pdf";
                mimeType = MimeTypes.PDF;
                exportData = HelperMethods.Export(users, ExportType.PDF);
                break;

            default:
                fileExtenstion = "xlsx";
                mimeType = MimeTypes.XLSX;
                exportData = HelperMethods.Export(users, ExportType.XLSX);
                break;
        }

        var result = File(exportData, mimeType, "Users." + fileExtenstion);

        return result;
    }
}