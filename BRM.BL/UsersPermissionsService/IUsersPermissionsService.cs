using System.Threading.Tasks;
using BRM.BL.Models;
using BRM.BL.Models.RoleDto;
using BRM.BL.Models.UserDto;
using BRM.BL.Models.UserPermissionDto;
using BRM.BL.Models.UserRoleDto;
using BRM.DAO.Entities;

namespace BRM.BL.UsersPermissionsService
{
    public interface IUsersPermissionsService
    {
        Task<UserPermissionReturnDto> AddPermissionToUser(long userId, long permissionId);
        Task<UserPermissionReturnDto> AddPermissionToUser(UserRoleOrPermissionUpdateDto dto);
        Task DeletePermissionFromUser(long userId, long permissionId);
        Task DeletePermissionFromUser(UserRoleOrPermissionUpdateDto dto);
        Task DeleteAllPermissionConnections(long permissionId);
        Task DeleteAllPermissionConnections(DeleteByIdDto dto);
        Task DeleteAllPermissionFromUser(long userId);
        Task DeleteAllPermissionFromUser(DeleteByIdDto dto);
        
    }
}