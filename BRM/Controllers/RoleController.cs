using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BRM.BL.Models.RoleDto;
using BRM.BL.Models.UserRoleDto;
using BRM.BL.RolesService;
using BRM.BL.UsersRolesService;
using Microsoft.AspNetCore.Mvc;

namespace BRM.Controllers
{
    [Route("roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRolesService _rolesService;
        private readonly IUsersRolesService _usersRolesService;

        public RoleController(IRolesService rolesService, IUsersRolesService usersRolesService)
        {
            _rolesService = rolesService;
            _usersRolesService = usersRolesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles(
        )
        {
            try
            {
                var user = await _rolesService.GetRolesAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }

        [HttpPost("role")]
        public async Task<IActionResult> AddRole(
            RoleAddDto dto
        )
        {
            try
            {
                var responsePayload = await _rolesService.AddRoleAsync(dto);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }

        [HttpPost("addRoleToUser")]
        public async Task<IActionResult> AddRoleToUser(
            UserRoleOrPermissionUpdateDto dto
        )
        {
            try
            {
                var responsePayload = await _usersRolesService.AddRoleToUserAsync(dto);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }

        [HttpPost("deleteRoleFromUser")]
        public async Task<IActionResult> RemoveRoleFromUser(
            UserRoleOrPermissionUpdateDto dto
        )
        {
            try
            {
                await _usersRolesService.DeleteRoleFromUserAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }


        [HttpDelete("role")]
        public async Task<IActionResult> RemoveRole(
            [Required] int id
        )
        {
            try
            {
                var responsePayload = await _rolesService.DeleteRoleAsync(id);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }

        [HttpPut("role")]
        public async Task<IActionResult> UpdateRole(
            RoleUpdateDto dto
        )
        {
            try
            {
                var responsePayload = await _rolesService.UpdateRoleAsync(dto);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}