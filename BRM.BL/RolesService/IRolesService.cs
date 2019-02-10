using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models;
using BRM.BL.Models.RoleDto;

namespace BRM.BL.RolesService
{
    public interface IRolesService
    {
        Task<RoleReturnDto> AddRoleAsync(RoleAddDto roleName);
        Task<List<RoleReturnDto>> GetRolesAsync();
        Task<RoleReturnDto> DeleteRoleAsync(long roleId);
        Task<RoleReturnDto> DeleteRoleAsync(DeleteByIdDto dto);
        Task<RoleReturnDto> UpdateRoleAsync(RoleUpdateDto model);

    }
}