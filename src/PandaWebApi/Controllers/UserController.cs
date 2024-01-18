using Microsoft.AspNetCore.Mvc;
using PandaFileExporter;
using PandaTech.IEnumerableFilters.Dto;
using PandaTech.IEnumerableFilters.Enums;
using PandaTech.ServiceResponse;
using PandaWebApi.Attributes;
using PandaWebApi.DTOs.User;
using PandaWebApi.Services.Interfaces;

namespace PandaWebApi.Controllers;

[ApiController]
[Route("api/v1/user")]
[Produces("application/json")]
[Authorize]
public class UserController(IUserService service) : Controller
{
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
    {
        await service.CreateUserAsync(createUserDto);
        return Ok();
    }


    [HttpPatch]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
    {
        await service.UpdateUserAsync(updateUserDto);
        return Ok();
    }

    [HttpPatch("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordDto updatePasswordDto)
    {
        await service.UpdatePasswordAsync(updatePasswordDto);
        return Ok();
    }

    [HttpPatch("status")]
    public async Task<IActionResult> UpdateUserStatus([FromBody] UpdateUserStatusDto updateUserStatusDto)
    {
        await service.UpdateUserStatusAsync(updateUserStatusDto);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUsers(List<long> ids)
    {
        await service.DeleteUsersAsync(ids);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers(int page, int pageSize, [FromQuery] string dataRequest)
    {
        var request = GetDataRequest.FromString(dataRequest);

        var data = await service.GetUsersAsync(page, pageSize, request);

        return Ok(data);
    }


    [HttpGet("filters")]
    public async Task<IActionResult> GetUserFilters()
    {
        var result = await service.GetUserFiltersAsync();
        var serviceResponse = new List<FilterInfo>(result);
        return Ok(serviceResponse);
    }

    [HttpGet("column-values")]
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

    [HttpGet("aggregate")]
    public async Task<IActionResult> Aggregate(string columnName, string filterString, AggregateType aggregate)
    {
        var output = await service.UserAggregateAsync(columnName, filterString, aggregate);
        return Ok(output);
    }

    // [HttpGet("export")]
    // public async Task<IActionResult> ExportUsers([FromQuery] string dataRequest,
    //     [FromQuery] ExportType exportType)
    // {
    //     string fileExtenstion;
    //     byte[] exportData;
    //     string mimeType;
    //
    //     var request = GetDataRequest.FromString(dataRequest);
    //
    //     var users = await service.ExportUsersAsync(request);
    //
    //     switch (exportType)
    //     {
    //         case ExportType.CSV:
    //             fileExtenstion = "csv";
    //             mimeType = MimeTypes.CSV;
    //             exportData = HelperMethods.Export(users, ExportType.CSV);
    //             break;
    //
    //         case ExportType.PDF:
    //             fileExtenstion = "pdf";
    //             mimeType = MimeTypes.PDF;
    //             exportData = HelperMethods.Export(users, ExportType.PDF);
    //             break;
    //
    //         default:
    //             fileExtenstion = "xlsx";
    //             mimeType = MimeTypes.XLSX;
    //             exportData = HelperMethods.Export(users, ExportType.XLSX);
    //             break;
    //     }
    //     var now = DateTime.UtcNow;
    //
    //     var result = File(exportData, mimeType, $"Users.{now}" + fileExtenstion);
    //
    //     return result;
    // }
}