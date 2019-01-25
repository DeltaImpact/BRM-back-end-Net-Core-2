using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BRM.BL.UserService;
using Microsoft.AspNetCore.Authorization;

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
            [Required] string userNickname
        )
        {
            try
            {
                var user = await _userService.GetUser(userNickname);
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
            [FromBody] [Required] string username
        )
        {
            try
            {
                var responsePayload = await _userService.AddUser(username);
                return Ok(responsePayload);
                //return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }
    }
}