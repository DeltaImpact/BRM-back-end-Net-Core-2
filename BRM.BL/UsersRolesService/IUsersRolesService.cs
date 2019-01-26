using System.Threading.Tasks;
using BRM.BL.Models.UserDto;
using BRM.BL.Models.UserRoleDto;

namespace BRM.BL.UsersRolesService
{
    public interface IUsersRolesService
    {
        Task<UserReturnDto> AddRoleToUser(UserRoleOrPermissionUpdateDto dto);
        Task<UserReturnDto> DeleteRoleFromUser(UserRoleOrPermissionUpdateDto dto);
        Task DeleteAllRoleConnections(long roleId);
        
    }
}