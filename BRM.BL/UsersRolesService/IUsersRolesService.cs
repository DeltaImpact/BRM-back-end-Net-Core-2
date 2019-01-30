using System.Threading.Tasks;
using BRM.BL.Models;
using BRM.BL.Models.UserRoleDto;

namespace BRM.BL.UsersRolesService
{
    public interface IUsersRolesService
    {
        Task<UserRoleReturnDto> AddRoleToUser(UserRoleOrPermissionUpdateDto dto);
        Task<UserRoleReturnDto> AddRoleToUser(long userId, long roleOrPermissionId);
        Task DeleteRoleFromUser(UserRoleOrPermissionUpdateDto dto);
        Task DeleteAllRoleConnections(long roleId);
        Task DeleteAllRoleFromUser(long userId);
        Task DeleteAllRoleConnections(DeleteByIdDto dto);
        Task DeleteAllRoleFromUser(DeleteByIdDto dto);
    }
}