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
                var user = await _userService.GetUserAsync(dto);
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
                var user = await _userService.GetUsersAsync();
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
                var newUser = await _userService.AddUserAsync(dto);

                if (dto.RolesId.Count == 0 && dto.PermissionsId.Count == 0)
                    return Ok(newUser);

                var roles = await _usersRolesService.AddRolesToUserAsync(newUser.Id, dto.RolesId);
                var permissions = await _usersPermissionsService.AddPermissionsToUserAsync(newUser.Id, dto.PermissionsId);
                newUser.Roles = roles.Select(o => o.Role).ToList();
                newUser.Permissions = permissions.Select(o => o.Permission).ToList();
                return Ok(newUser);
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
                await _userService.DeleteUserAsync(id);
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