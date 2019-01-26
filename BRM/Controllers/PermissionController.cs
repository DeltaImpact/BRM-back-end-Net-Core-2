using System;
using System.Threading.Tasks;
using BRM.BL.Models.RoleDto;
using BRM.BL.Models.UserRoleDto;
using BRM.BL.PermissionsService;
using BRM.BL.UsersPermissionsService;
using Microsoft.AspNetCore.Mvc;

namespace BRM.Controllers
{
    [Route("permission")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IUsersPermissionsService _usersPermissionsService;

        public PermissionController(IPermissionsService permissionsService, IUsersPermissionsService usersPermissionsService)
        {
            _permissionsService = permissionsService;
            _usersPermissionsService = usersPermissionsService;
        }

        [HttpGet("permissions")]
        public async Task<IActionResult> GetPermissions(
        )
        {
            try
            {
                var user = await _permissionsService.GetPermissions();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }

        [HttpPost("addPermission")]
        public async Task<IActionResult> AddPermission(
            PermissionAddDto dto
        )
        {
            try
            {
                var responsePayload = await _permissionsService.AddPermission(dto);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }


        [HttpPost("addPermissionToUser")]
        public async Task<IActionResult> AddPermissionToUser(
            [FromBody] UserRoleOrPermissionUpdateDto dto
        )
        {
            try
            {
                var responsePayload = await _usersPermissionsService.AddPermissionToUser(dto);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("deletePermissionFromUser")]
        public async Task<IActionResult> RemovePermissionFromUser(
            [FromBody] UserRoleOrPermissionUpdateDto dto
        )
        {
            try
            {
                var responsePayload = await _usersPermissionsService.DeletePermissionFromUser(dto);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}