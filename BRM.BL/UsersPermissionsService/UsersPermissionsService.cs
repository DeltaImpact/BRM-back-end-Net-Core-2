using System.Threading.Tasks;
using BackSide2.BL.Exceptions;
using BRM.BL.Extensions.UserDtoExtensions;
using BRM.BL.Models.UserDto;
using BRM.BL.Models.UserPermissionDto;
using BRM.BL.Models.UserRoleDto;
using BRM.BL.UserService;
using BRM.DAO.Entities;
using BRM.DAO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BRM.BL.UsersPermissionsService
{
    public class UsersPermissionsService : IUsersPermissionsService
    {
        private readonly IRepository<UsersRoles> _usersRoles;
        private readonly IRepository<User> _userService;
        private readonly IRepository<UsersPermissions> _usersPermissions;
        private readonly IRepository<Role> _roleService;
        private readonly IRepository<Permission> _permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersPermissionsService(
            IRepository<User> userService,
            IRepository<UsersPermissions> usersPermissions,
            IRepository<UsersRoles> usersRoles,
            IRepository<Role> roleService,
            IRepository<Permission> permissionService,
            IHttpContextAccessor httpContextAccessor)
        {
            _usersRoles = usersRoles;
            _userService = userService;
            _usersPermissions = usersPermissions;
            _roleService = roleService;
            _permissionService = permissionService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserPermissionReturnDto> AddPermissionToUser(long userId, long permissionId)
        {
            var user =
                await _userService.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var permission =
                await _permissionService.GetByIdAsync(permissionId);

            if (permission == null)
            {
                throw new ObjectNotFoundException("Permission not found.");
            }

            var userToRoleConnection =
                await (await _usersPermissions.GetAllAsync(d => d.User == user && d.Permission == permission))
                    .FirstOrDefaultAsync();
            
            if (userToRoleConnection != null)
            {
                throw new ObjectNotFoundException("User already have role.");
            }

            var userToRoleForDb = new UsersPermissions
            {
                User = user,
                Permission = permission
            };

            var connection = (await _usersPermissions.InsertAsync(userToRoleForDb));
            
            return connection.ToUserPermissionReturnDto();
        }

        public async Task<UserPermissionReturnDto> DeletePermissionFromUser(long userId, long permissionId)
        {
            var user =
                await _userService.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var role =
                await _usersPermissions.GetByIdAsync(permissionId);

            if (role == null)
            {
                throw new ObjectNotFoundException("User permission not found.");
            }

            var userToRoleConnection =
                await(await _usersPermissions.GetAllAsync(d => d.User == user && d.Permission == role.Permission))
                    .FirstOrDefaultAsync();

            if (userToRoleConnection == null)
            {
                throw new ObjectNotFoundException("User permission not found.");
            }

            var removedRole = await _usersPermissions.RemoveAsync(userToRoleConnection);
            return removedRole.ToUserPermissionReturnDto();
        }

        public async Task DeleteAllPermissionConnections(long permissionId)
        {
            var permissions =
                await _permissionService.GetByIdAsync(permissionId);

            if (permissions == null)
            {
                throw new ObjectNotFoundException("Permission not found.");
            }

            var allRoleConnections =
                await(await _usersPermissions.GetAllAsync(d => d.Permission == permissions)).ToListAsync();

            foreach (var roleConnection in allRoleConnections)
            {
                await _usersPermissions.RemoveAsync(roleConnection);
            }

        }
    }
}