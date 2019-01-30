using System.Collections.Generic;
using BRM.BL.Models.PermissionDto;
using BRM.BL.Models.RoleDto;
using BRM.BL.Models.UserDto;
using BRM.DAO.Entities;

namespace BRM.BL.Extensions.UserDtoExtensions
{
    static class UserReturnDtoExtensions
    {
        public static UserReturnDto ToUserReturnDto(this User user, List<PermissionReturnDto> permissions, List<RoleReturnDto> roles)
        {
            return new UserReturnDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Permissions = permissions,
                Roles = roles,
            };
        }

        public static UserReturnDto ToUserReturnDto(this User user)
        {
            return new UserReturnDto
            {
                Id = user.Id,
                UserName = user.UserName
            };
        }
    }
}
