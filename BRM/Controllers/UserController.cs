using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BRM.BL.Models.RoleDto;
using BRM.BL.Models.UserDto;
using Microsoft.AspNetCore.Mvc;
using BRM.BL.UserService;

namespace BRM.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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
                return Ok(responsePayload);
                //return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }

        [HttpDelete("deleteUser")]
        public async Task<IActionResult> DeleteUser(
            DeleteByIdDto dto
        )
        {
            try
            {

                //var responsePayload = await _userService.AddUser(dto);
                //return Ok(responsePayload);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}