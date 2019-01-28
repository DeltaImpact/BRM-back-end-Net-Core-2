using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models;
using BRM.BL.Models.PermissionDto;
using BRM.BL.Models.RoleDto;

namespace BRM.BL.PermissionsService
{
    public interface IPermissionsService
    {
        Task<PermissionReturnDto> AddPermission(PermissionAddDto dto);
        Task<List<PermissionReturnDto>> GetPermissions();
        Task<PermissionReturnDto> DeletePermission(long permissionId);
        Task<PermissionReturnDto> DeletePermission(DeleteByIdDto dto);
        Task<PermissionReturnDto> UpdatePermissionAsync(PermissionUpdateDto model);
    }
}