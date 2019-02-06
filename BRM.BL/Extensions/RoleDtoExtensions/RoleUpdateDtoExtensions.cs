using BRM.BL.Models.RoleDto;
using BRM.DAO.Entities;

namespace BRM.BL.Extensions.RoleDtoExtensions
{
    public static class RoleUpdateDtoExtensions
    {
        public static Role ToRole(this RoleUpdateDto model)
        {
            var role = new Role
            {
                Name = model.Name,
                Id = model.Id
            };
            return role;
        }

        public static Role ToRole(this RoleUpdateDto model, Role roleOld)
        {
            roleOld.Name = model.Name;
            return roleOld;
        }
    }
}
