using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models;
using BRM.BL.Models.PermissionDto;
using BRM.BL.Models.RoleDto;

namespace BRM.BL.RolesService
{
    public interface IRolesService
    {
        Task<PermissionReturnDto> AddRole(RoleAddDto roleName);
        Task<List<PermissionReturnDto>> GetRoles();
        Task<PermissionReturnDto> DeleteRole(long roleId);
        Task<PermissionReturnDto> DeleteRole(DeleteByIdDto dto);
        Task<PermissionReturnDto> UpdateRoleAsync(RoleUpdateDto model);

    }
}