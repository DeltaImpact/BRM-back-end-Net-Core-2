using System.Threading.Tasks;
using BRM.BL.Models.RoleDto;
using BRM.BL.Models.UserDto;
using BRM.BL.Models.UserPermissionDto;
using BRM.BL.Models.UserRoleDto;

namespace BRM.BL.UsersPermissionsService
{
    public interface IUsersPermissionsService
    {
        Task<UserReturnDto> AddPermissionToUser(long userId, long permissionId);
        Task<UserReturnDto> DeletePermissionFromUser(long userId, long permissionId);
        Task<UserReturnDto> AddPermissionToUser(UserRoleOrPermissionUpdateDto dto);
        Task<UserReturnDto> DeletePermissionFromUser(UserRoleOrPermissionUpdateDto dto);
        Task DeleteAllPermissionConnections(long permissionId);
        Task DeleteAllPermissionFromUser(long userId);
        Task DeleteAllPermissionConnections(DeleteByIdDto dto);
        Task DeleteAllPermissionFromUser(DeleteByIdDto dto);
        
    }
}