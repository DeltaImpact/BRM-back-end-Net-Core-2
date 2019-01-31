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
        Task<UserPermissionReturnDto> AddPermissionToUser(long userId, long permissionId);
        Task<UserPermissionReturnDto> AddPermissionToUser(UserRoleOrPermissionUpdateDto dto);
        Task<List<UserPermissionReturnDto>> AddPermissionsToUser(long userId, ICollection<long> permissionsId);
        Task DeletePermissionFromUser(long userId, long permissionId);
        Task DeletePermissionFromUser(UserRoleOrPermissionUpdateDto dto);
        Task DeleteAllPermissionConnections(long permissionId);
        Task DeleteAllPermissionConnections(DeleteByIdDto dto);
        Task DeleteAllPermissionFromUser(long userId);
        Task DeleteAllPermissionFromUser(DeleteByIdDto dto);
        
    }
}