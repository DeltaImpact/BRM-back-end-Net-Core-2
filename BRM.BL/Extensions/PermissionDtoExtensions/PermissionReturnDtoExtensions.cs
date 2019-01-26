using BRM.BL.Models.PermissionDto;
using BRM.DAO.Entities;

namespace BRM.BL.Extensions.PermissionDtoExtensions
{
    static class PermissionReturnDtoExtensions
    {
        public static PermissionReturnDto ToPermissionReturnDto(this Permission role)
        {
            return new PermissionReturnDto
             {
                Id = role.Id,
                Name = role.Name
            };
        }
    }
}
