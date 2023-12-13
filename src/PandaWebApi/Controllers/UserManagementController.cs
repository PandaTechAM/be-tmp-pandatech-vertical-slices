using Microsoft.AspNetCore.Mvc;
using PandaFileExporter;
using PandaTech.IEnumerableFilters.Dto;
using PandaTech.ServiceResponse;
using PandaWebApi.DTOs.Authentication;
using PandaWebApi.DTOs.UserManagement;
using PandaWebApi.Enums;
using PandaWebApi.Filters;
using PandaWebApi.Helpers;
using PandaWebApi.Services.Interfaces;

namespace PandaWebApi.Controllers;

[ApiController]
[Route("api/v1")]
[Produces("application/json")]
[Authorize(Roles.User)]
public class UserManagementController : Controller
{
    private readonly IUserManagementService _userManagementService;

    public UserManagementController(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }
    
    [UnAuthorize]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginDto loginDto)
    {
        var response = await _userManagementService.LoginAsync(loginDto, HttpContext);

        return Ok(response);
    }
    
    
        [Authorize(Roles.Admin)]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(int page, int pageSize, [FromQuery] string dataRequest)
        {
            var request = GetDataRequest.FromString(dataRequest);

            var data = await _userManagementService.GetUsersAsync(page, pageSize, request);

            return Ok(data);
        }

        [Authorize(Roles.User)]
        [HttpGet("user-identify")]
        public IActionResult GetUser([FromHeader] Guid token)
        {
            var data = _userManagementService.IdentifyUser();

            return Ok(data);
        }
        
        [Authorize(Roles.Admin)]
        [HttpPost("user")]
        public async Task<IActionResult> CreateUser(AddUserDto addUserDto)
        {
            await _userManagementService.CreateUserAsync(addUserDto);
            return Ok();
        }

        [Authorize(Roles.Admin)]
        [HttpDelete("users")]
        public async Task<IActionResult> DeleteUsers(List<long> ids)
        {
            await _userManagementService.DeleteUsersAsync(ids);
            return Ok();
        }

        [Authorize(Roles.Admin)]
        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            await _userManagementService.UpdatePasswordAsync(changePasswordDto);
            return Ok();
        }

        [Authorize(Roles.Admin)]
        [HttpPatch("user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            await _userManagementService.UpdateUserAsync(updateUserDto);
            return Ok();
        }

        [Authorize(Roles.Admin)]
        [HttpPatch("user-status")]
        public async Task<IActionResult> UpdateUserStatus([FromBody] UpdateUserStatusDto updateUserStatusDto)
        {
            await _userManagementService.UpdateUserStatusAsync(updateUserStatusDto);
            return Ok();
        }

        [Authorize(Roles.User)]

        [HttpPatch("change-own-password")]
        public async Task<IActionResult> ChangeOwnPassword([FromBody] ChangeOwnPasswordDto changePasswordDto)
        {
            await _userManagementService.ChangeOwnPasswordAsync(changePasswordDto);
            return Ok();
        }

        [Authorize(Roles.User)]

        [HttpPatch("change-own-password-forced")]
        public async Task<IActionResult> ChangeOwnPasswordForced([FromBody] ForcedPasswordChangeDto password)
        {
            await _userManagementService.ForcePasswordChangeAsync(password.Password);
            return Ok();
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
        
            var users = await _userManagementService.GetAllUsersForExportAsync(request);
        
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

        [HttpGet("user-filters")]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> GetUserFilters()
        {
            var result = await _userManagementService.GetUserFiltersAsync();
            var serviceResponse = new List<FilterInfo>(result);
            return Ok(serviceResponse);
        }
        
        [HttpGet("user-column-values")]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> ColumnValues(string columnName, string filterString,
            int page, int pageSize)
        {
            var output = await _userManagementService.UserColumnValuesAsync(columnName, filterString, page, pageSize);

            var responseData = new ResponseDataPaged<object>
            {
                Data = output.Values,
                TotalCount = output.TotalCount,
                Page = page,
                PageSize = pageSize
            };
            return Ok(responseData);
        }
}