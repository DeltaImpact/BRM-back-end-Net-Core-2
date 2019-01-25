using BRM.BL.Extensions.RoleDtoExtensions;
using BRM.BL.Extensions.UserDtoExtensions;
using BRM.BL.Models.UserRoleDto;
using BRM.DAO.Entities;

namespace BRM.BL.Extensions.UserRoleExtensions
{
    static class UserRoleReturnDtoExtensions
    {
        public static UserRoleReturnDto ToUserRoleReturnDto(this UsersRoles userRole)
        {
            return new UserRoleReturnDto
            {
                User = userRole.User.ToUserReturnDto(),
                Role = userRole.Role.ToRoleReturnDto()
            };
        }
    }
}
