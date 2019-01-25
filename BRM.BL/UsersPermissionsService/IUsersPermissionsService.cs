using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models.UserDto;
using BRM.BL.Models.UserPermissionDto;
using BRM.BL.Models.UserRoleDto;

namespace BRM.BL.UserService
{
    public interface IUsersPermissionsService
    {
        Task<UserPermissionReturnDto> AddPermissionToUser(long userId, long permissionId);
        Task<UserPermissionReturnDto> DeletePermissionFromUser(long userId, long permissionId);
        Task DeleteAllPermissionConnections(long permissionId);
        
    }
}