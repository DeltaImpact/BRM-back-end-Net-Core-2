using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models.PermissionDto;
using BRM.BL.Models.RoleDto;

namespace BRM.BL.PermissionsService
{
    public interface IPermissionsService
    {
        Task<PermissionReturnDto> AddPermission(PermissionAddDto dto);
        Task<List<PermissionReturnDto>> GetPermissions();
    }
}