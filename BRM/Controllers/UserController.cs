using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using BRM.BL.Models.UserDto;
using BRM.BL.UsersPermissionsService;
using BRM.BL.UsersRolesService;
using Microsoft.AspNetCore.Mvc;
using BRM.BL.UserService;
using UserAddDto = BRM.BL.Models.UserDto.UserAddDto;

namespace BRM.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUsersRolesService _usersRolesService;
        private readonly IUsersPermissionsService _usersPermissionsService;

        public UserController(IUserService userService, IUsersRolesService usersRolesService,
            IUsersPermissionsService usersPermissionsService)
        {
            _userService = userService;
            _usersRolesService = usersRolesService;
            _usersPermissionsService = usersPermissionsService;
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUser(
            UserAddDto dto
        )
        {
            try
            {
                var user = await _userService.GetUser(dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(
        )
        {
            try
            {
                var user = await _userService.GetUsers();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            UserAddDto dto
        )
        {
            try
            {
                var responsePayload = await _userService.AddUser(dto);

                if (dto.RolesId.Count == 0 && dto.PermissionsId.Count == 0)
                    return Ok(responsePayload);

                foreach (var roleId in dto.RolesId)
                {
                    await _usersRolesService.AddRoleToUser(responsePayload.Id, roleId);
                }
                foreach (var permissionsId in dto.PermissionsId)
                {
                    await _usersPermissionsService.AddPermissionToUser(responsePayload.Id, permissionsId);
                }
                
                //var rolesTasks = dto.RolesId.Select(o => _usersRolesService.AddRoleToUser(responsePayload.Id, o))
                //    .ToArray();
                //var permissionsTasks = dto.PermissionsId
                //    .Select(o => _usersPermissionsService.AddPermissionToUser(responsePayload.Id, o)).ToArray();

                //await Task.WhenAll(rolesTasks);
                //await Task.WhenAll(permissionsTasks);
                var user = await _userService.GetUserById(responsePayload.Id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }

        [HttpDelete("deleteUser")]
        public async Task<IActionResult> DeleteUser(
            [Required] int id
        )
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok();
                //return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateRole(
            UserUpdateDto dto
        )
        {
            try
            {
                var responsePayload = await _userService.UpdateUserAsync(dto);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }
    }
}