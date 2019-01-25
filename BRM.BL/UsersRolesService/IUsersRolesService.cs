using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models.UserDto;
using BRM.BL.Models.UserRoleDto;

namespace BRM.BL.UserService
{
    public interface IUsersRolesService
    {
        Task<UserReturnDto> AddRoleToUser(UserRoleUpdateDto dto);
        Task<UserReturnDto> DeleteRoleFromUser(UserRoleUpdateDto dto);
        Task DeleteAllRoleConnections(long roleId);
        
    }
}