﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BRM.BL.Models.PermissionDto;
using BRM.BL.Models.UserRoleDto;
using BRM.BL.PermissionsService;
using BRM.BL.UsersPermissionsService;
using Microsoft.AspNetCore.Mvc;

namespace BRM.Controllers
{
    [Route("permissions")]
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

        [HttpGet]
        public async Task<IActionResult> GetPermissions(
        )
        {
            try
            {
                var user = await _permissionsService.GetPermissionsAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }

        [HttpPost("permission")]
        public async Task<IActionResult> AddPermission(
            PermissionAddDto dto
        )
        {
            try
            {
                var responsePayload = await _permissionsService.AddPermissionAsync(dto);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
        }


        [HttpPut("permission")]
        public async Task<IActionResult> UpdatePermission(
            PermissionUpdateDto model
        )
        {
            try
            {
                var responsePayload = await _permissionsService.UpdatePermissionAsync(model);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }


        [HttpPost("addPermissionToUser")]
        public async Task<IActionResult> AddPermissionToUser(
            UserRoleOrPermissionUpdateDto dto
        )
        {
            try
            {
                var responsePayload = await _usersPermissionsService.AddPermissionToUserAsync(dto);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("deletePermissionFromUser")]
        public async Task<IActionResult> RemovePermissionFromUser(
            UserRoleOrPermissionUpdateDto dto
        )
        {
            try
            {
                await _usersPermissionsService.DeletePermissionFromUserAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }


        [HttpDelete("permission")]
        public async Task<IActionResult> RemovePermission(
            [Required] int id
        )
        {
            try
            {
                var responsePayload = await _permissionsService.DeletePermissionAsync(id);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}