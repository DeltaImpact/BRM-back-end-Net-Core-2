using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BRM.BL.PermissionsService;
using BRM.BL.RolesService;
using Microsoft.AspNetCore.Mvc;
using BRM.BL.UserService;
using Microsoft.AspNetCore.Authorization;

namespace BRM.Controllers
{
    [Route("permission")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionsService _permissionsService;

        public PermissionController(IPermissionsService permissionsService)
        {
            _permissionsService = permissionsService;
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
            [FromBody] [Required] string permissionName
        )
        {
            try
            {
                var responsePayload = await _permissionsService.AddPermission(permissionName);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }
    }
}