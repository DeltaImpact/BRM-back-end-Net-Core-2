using BRM.BL.Extensions.PermissionDtoExtensions;
using BRM.BL.Extensions.UserDtoExtensions;
using BRM.BL.Models.UserPermissionDto;
using BRM.DAO.Entities;

namespace BRM.BL.Extensions.UserPermissionExtensions
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
