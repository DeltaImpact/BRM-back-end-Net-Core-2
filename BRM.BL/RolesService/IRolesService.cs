using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models.UserDto;

namespace BRM.BL.RolesService
{
    public interface IRolesService
    {
        Task<PermissionReturnDto> AddRole(string roleName);
        Task<List<PermissionReturnDto>> GetRoles();
        Task<PermissionReturnDto> DeleteRole(long roleId);
        
    }
}