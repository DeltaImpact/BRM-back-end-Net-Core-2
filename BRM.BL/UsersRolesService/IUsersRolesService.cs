using System.Threading.Tasks;
using BRM.BL.Models;
using BRM.BL.Models.RoleDto;
using BRM.BL.Models.UserDto;
using BRM.BL.Models.UserRoleDto;
using BRM.DAO.Entities;

namespace BRM.BL.UsersRolesService
{
    public interface IUsersRolesService
    {
        Task<UserRoleReturnDto> AddRoleToUser(UserRoleOrPermissionUpdateDto dto);
        Task DeleteRoleFromUser(UserRoleOrPermissionUpdateDto dto);
        Task DeleteAllRoleConnections(long roleId);
        Task DeleteAllRoleFromUser(long userId);
        Task DeleteAllRoleConnections(DeleteByIdDto dto);
        Task DeleteAllRoleFromUser(DeleteByIdDto dto);
    }
}