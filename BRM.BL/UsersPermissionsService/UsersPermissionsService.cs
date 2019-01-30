using System.Threading.Tasks;
using BRM.BL.Exceptions;
using BRM.BL.Extensions.UserPermissionExtensions;
using BRM.BL.Models;
using BRM.BL.Models.UserPermissionDto;
using BRM.BL.Models.UserRoleDto;
using BRM.DAO.Entities;
using BRM.DAO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BRM.BL.UsersPermissionsService
{
    public class UsersPermissionsService : IUsersPermissionsService
    {
        public IRepository<UsersPermissions> UsersPermissionsRepository { get; }
        public IRepository<UsersRoles> UsersRolesRepository { get; }
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UsersPermissions> _usersPermissions;
        private readonly IRepository<Permission> _permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersPermissionsService(
            IRepository<User> userRepository,
            IRepository<UsersPermissions> usersPermissions,
            IRepository<Permission> permissionService,
            IHttpContextAccessor httpContextAccessor,
            IRepository<UsersPermissions> usersPermissionsRepository,
            IRepository<UsersRoles> usersRolesRepository
        )
        {
            UsersPermissionsRepository = usersPermissionsRepository;
            UsersRolesRepository = usersRolesRepository;
            _userRepository = userRepository;
            _usersPermissions = usersPermissions;

            _permissionService = permissionService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserPermissionReturnDto> AddPermissionToUser(long userId, long permissionId)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);

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
                throw new ObjectNotFoundException("User already have permission.");
            }

            var userToRoleForDb = new UsersPermissions
            {
                User = user,
                Permission = permission
            };

            var connection = (await _usersPermissions.InsertAsync(userToRoleForDb));
            return connection.ToUserPermissionReturnDto();
            //return await _userService.GetUser(connection.User.UserName);
        }

        public async Task DeletePermissionFromUser(long userId, long permissionId)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var userToPermissionConnection =
                await (await _usersPermissions.GetAllAsync(d => d.User == user && d.Permission.Id == permissionId, i => i.User))
                    .FirstOrDefaultAsync();

            if (userToPermissionConnection == null)
            {
                throw new ObjectNotFoundException("User permission not found.");
            }

            var removedPermission = await _usersPermissions.RemoveAsync(userToPermissionConnection);
            //return removedPermission.ToUserPermissionReturnDto();
            //return await _userService.GetUser(removedPermission.User.UserName);
        }

        public Task<UserPermissionReturnDto> AddPermissionToUser(UserRoleOrPermissionUpdateDto dto)
        {
            return AddPermissionToUser(dto.UserId, dto.RoleOrPermissionId);
        }

        public Task DeletePermissionFromUser(UserRoleOrPermissionUpdateDto dto)
        {
            return DeletePermissionFromUser(dto.UserId, dto.RoleOrPermissionId);
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
                await (await _usersPermissions.GetAllAsync(d => d.Permission == permissions)).ToListAsync();

            foreach (var roleConnection in allRoleConnections)
            {
                await _usersPermissions.RemoveAsync(roleConnection);
            }
        }

        public async Task DeleteAllPermissionFromUser(long userId)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var allPermissionToUserConnections =
                await (await _usersPermissions.GetAllAsync(d => d.User.Id == userId)).ToListAsync();

            foreach (var userConnection in allPermissionToUserConnections)
            {
                await _usersPermissions.RemoveAsync(userConnection);
            }
        }

        public async Task DeleteAllPermissionConnections(DeleteByIdDto dto)
        {
            await DeleteAllPermissionConnections(dto.Id);
        }

        public async Task DeleteAllPermissionFromUser(DeleteByIdDto dto)
        {
            await DeleteAllPermissionFromUser(dto.Id);
        }

    }
}