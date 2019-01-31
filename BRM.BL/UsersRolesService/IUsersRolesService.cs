using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models;
using BRM.BL.Models.RoleDto;
using BRM.BL.Models.UserRoleDto;

namespace BRM.BL.UsersRolesService
{
    public interface IUsersRolesService
    {
        Task<UserRoleReturnDto> AddRoleToUser(UserRoleOrPermissionUpdateDto dto);
        Task<UserRoleReturnDto> AddRoleToUser(long userId, long roleOrPermissionId);
        Task<List<UserRoleReturnDto>> AddRolesToUser(long userId, ICollection<long> rolesId);
        Task DeleteRoleFromUser(UserRoleOrPermissionUpdateDto dto);
        Task DeleteAllRoleConnections(long roleId);
        Task DeleteAllRoleFromUser(long userId);
        Task DeleteAllRoleConnections(DeleteByIdDto dto);
        Task DeleteAllRoleFromUser(DeleteByIdDto dto);
    }
}