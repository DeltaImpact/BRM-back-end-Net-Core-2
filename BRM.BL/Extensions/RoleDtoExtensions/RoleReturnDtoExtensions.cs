using BRM.BL.Models.RoleDto;
using BRM.DAO.Entities;

namespace BRM.BL.Extensions.RoleDtoExtensions
{
    static class RoleReturnDtoExtensions
    {
        public static RoleReturnDto ToRoleReturnDto(this Role role)
        {
            return new RoleReturnDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }
    }
}
