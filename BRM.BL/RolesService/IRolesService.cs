using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models;
using BRM.BL.Models.RoleDto;

namespace BRM.BL.RolesService
{
    public interface IRolesService
    {
        Task<RoleReturnDto> AddRole(RoleAddDto roleName);
        Task<List<RoleReturnDto>> GetRoles();
        Task<RoleReturnDto> DeleteRole(long roleId);
        Task<RoleReturnDto> DeleteRole(DeleteByIdDto dto);
        Task<RoleReturnDto> UpdateRoleAsync(RoleUpdateDto model);

    }
}