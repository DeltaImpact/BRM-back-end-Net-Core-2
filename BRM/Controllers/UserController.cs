﻿using System;
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
    [Route("users")]
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

        [HttpGet]
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

        [HttpPost("user")]
        public async Task<IActionResult> Register(
            UserAddDto dto
        )
        {
            try
            {
                var newUser = await _userService.AddUserAsync(dto);
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }

        [HttpDelete("user")]
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


        [HttpPut("user")]
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