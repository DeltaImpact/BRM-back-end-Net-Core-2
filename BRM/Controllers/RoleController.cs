﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BRM.BL.Models.UserRoleDto;
using BRM.BL.RolesService;
using Microsoft.AspNetCore.Mvc;
using BRM.BL.UserService;
using Microsoft.AspNetCore.Authorization;

namespace BRM.Controllers
{
    [Route("role")]
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

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles(
        )
        {
            try
            {
                var user = await _rolesService.GetRoles();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }

        [HttpPost("addRole")]
        public async Task<IActionResult> AddRole(
            [FromBody] [Required] string roleName
        )
        {
            try
            {
                var responsePayload = await _rolesService.AddRole(roleName);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }

        [HttpPost("addRoleToUser")]
        public async Task<IActionResult> AddRoleToUser(
            [FromBody] UserRoleUpdateDto dto
        )
        {
            try
            {
                var responsePayload = await _usersRolesService.AddRoleToUser(dto);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("deleteRoleFromUser")]
        public async Task<IActionResult> RemoveRoleFromUser(
            [FromBody] UserRoleUpdateDto dto
        )
        {
            try
            {
                var responsePayload = await _usersRolesService.DeleteRoleFromUser(dto);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}