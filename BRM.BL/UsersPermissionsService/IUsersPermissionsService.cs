using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models;
using BRM.BL.Models.PermissionDto;
using BRM.BL.Models.RoleDto;
using BRM.BL.Models.UserPermissionDto;
using BRM.BL.Models.UserRoleDto;

namespace BRM.BL.UsersPermissionsService
{
    public interface IUsersPermissionsService
    {
        Task<UserPermissionReturnDto> AddPermissionToUserAsync(long userId, long permissionId);
        Task<UserPermissionReturnDto> AddPermissionToUserAsync(UserRoleOrPermissionUpdateDto dto);
        Task<List<UserPermissionReturnDto>> AddPermissionsToUserAsync(long userId, ICollection<long> permissionsId);
        Task DeletePermissionFromUserAsync(long userId, long permissionId);
        Task DeletePermissionFromUserAsync(UserRoleOrPermissionUpdateDto dto);
        Task DeleteAllPermissionConnectionsAsync(long permissionId);
        Task DeleteAllPermissionConnectionsAsync(DeleteByIdDto dto);
        Task DeleteAllPermissionFromUserAsync(long userId);
        Task DeleteAllPermissionFromUserAsync(DeleteByIdDto dto);
        
    }
}