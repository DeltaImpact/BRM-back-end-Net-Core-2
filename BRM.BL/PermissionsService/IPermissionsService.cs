using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models;
using BRM.BL.Models.PermissionDto;

namespace BRM.BL.PermissionsService
{
    public interface IPermissionsService
    {
        Task<PermissionReturnDto> AddPermissionAsync(PermissionAddDto dto);
        Task<List<PermissionReturnDto>> GetPermissionsAsync();
        Task<PermissionReturnDto> DeletePermissionAsync(long permissionId);
        Task<PermissionReturnDto> DeletePermissionAsync(DeleteByIdDto dto);
        Task<PermissionReturnDto> UpdatePermissionAsync(PermissionUpdateDto model);
    }
}