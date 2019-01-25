using System;
using System.Collections.Generic;
using System.Text;
using BRM.BL.Extensions.PermissionDtoExtensions;
using BRM.BL.Models.UserDto;
using BRM.BL.Models.UserPermissionDto;
using BRM.BL.Models.UserRoleDto;
using BRM.DAO.Entities;

namespace BRM.BL.Extensions.UserDtoExtensions
{
    static class UserPermissionExtensions
    {
        public static UserPermissionReturnDto ToUserPermissionReturnDto(this UsersPermissions userRole)
        {
            return new UserPermissionReturnDto
            {
                User = userRole.User.ToUserReturnDto(),
                Permission = userRole.Permission.ToPermissionReturnDto()
            };
        }
    }
}
