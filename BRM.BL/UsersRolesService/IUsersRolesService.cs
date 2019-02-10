using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models;
using BRM.BL.Models.RoleDto;
using BRM.BL.Models.UserRoleDto;

namespace BRM.BL.UsersRolesService
{
    public interface IUsersRolesService
    {
        Task<UserRoleReturnDto> AddRoleToUserAsync(UserRoleOrPermissionUpdateDto dto);
        Task<UserRoleReturnDto> AddRoleToUserAsync(long userId, long roleOrPermissionId);
        Task<List<UserRoleReturnDto>> AddRolesToUserAsync(long userId, ICollection<long> rolesId);
        Task DeleteRoleFromUserAsync(UserRoleOrPermissionUpdateDto dto);
        Task DeleteAllRoleConnectionsAsync(long roleId);
        Task DeleteAllRoleFromUserAsync(long userId);
        Task DeleteAllRoleConnectionsAsync(DeleteByIdDto dto);
        Task DeleteAllRoleFromUserAsync(DeleteByIdDto dto);
    }
}