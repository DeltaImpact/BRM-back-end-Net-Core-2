using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models.UserDto;

namespace BRM.BL.PermissionsService
{
    public interface IPermissionsService
    {
        Task<PermissionReturnDto> AddPermission(string permissionName);
        Task<List<PermissionReturnDto>> GetPermissions();
    }
}