using BRM.BL.Models.PermissionDto;
using BRM.DAO.Entities;

namespace BRM.BL.Extensions.PermissionDtoExtensions
{
    public static class PermissionUpdateDtoExtensions
    {
        public static Permission ToPermission(this PermissionUpdateDto model)
        {
            var permission = new Permission
            {
                Name = model.Name,
                Id = model.Id
            };
            return permission;
        }
    }
}
