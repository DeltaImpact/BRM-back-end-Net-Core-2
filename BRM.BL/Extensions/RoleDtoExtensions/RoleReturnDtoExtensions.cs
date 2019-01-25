using BRM.BL.Models.UserDto;
using BRM.DAO.Entities;

namespace BRM.BL.Extensions.RoleDtoExtensions
{
    static class RoleReturnDtoExtensions
    {
        public static PermissionReturnDto ToRoleReturnDto(this Role role)
        {
            return new PermissionReturnDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }
    }
}
