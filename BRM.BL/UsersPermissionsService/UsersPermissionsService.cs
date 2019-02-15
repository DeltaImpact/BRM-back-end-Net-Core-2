using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BRM.BL.Exceptions;
using BRM.BL.Extensions.PermissionDtoExtensions;
using BRM.BL.Extensions.UserPermissionExtensions;
using BRM.BL.Models;
using BRM.BL.Models.PermissionDto;
using BRM.BL.Models.RoleDto;
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
        private readonly IRepository<UsersPermissions> _usersPermissionsRepository;
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersPermissionsService(
            IRepository<User> userRepository,
            IRepository<Permission> permissionRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<UsersPermissions> usersPermissionsRepository,
            IRepository<UsersRoles> usersRolesRepository
        )
        {
            UsersRolesRepository = usersRolesRepository;
            _userRepository = userRepository;
            _usersPermissionsRepository = usersPermissionsRepository;

            _permissionRepository = permissionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserPermissionReturnDto> AddPermissionToUserAsync(long userId, long permissionId)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var permission =
                await _permissionRepository.GetByIdAsync(permissionId);

            if (permission == null)
            {
                throw new ObjectNotFoundException("Permission not found.");
            }

            var userToRoleConnection =
                await (await _usersPermissionsRepository.GetAllAsync(d => d.User == user && d.Permission == permission))
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

            var connection = (await _usersPermissionsRepository.InsertAsync(userToRoleForDb));
            connection.User = user;
            connection.Permission = permission;
            return connection.ToUserPermissionReturnDto();
        }

        public async Task<List<UserPermissionReturnDto>> AddPermissionsToUserAsync(long userId,
            ICollection<long> permissionsId)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var roles = await _permissionRepository.GetAllAsync(o => permissionsId.Contains(o.Id));
            foreach (var role in roles)
            {
                if (!permissionsId.Contains(role.Id)) throw new ObjectNotFoundException("Permission not found.");
            }

            var toInsert = roles.Select(permission =>
                new UsersPermissions()
                {
                    Permission = permission,
                    User = user
                }).ToArray();

            var connections = (await _usersPermissionsRepository.InsertManyAsync(toInsert))
                .Select(o => o.ToUserPermissionReturnDto()).ToList();
            return connections;
        }

        public async Task DeletePermissionFromUserAsync(long userId, long permissionId)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var userToPermissionConnection =
                await (await _usersPermissionsRepository.GetAllAsync(
                        d => d.User == user && d.Permission.Id == permissionId,
                        i => i.User))
                    .FirstOrDefaultAsync();

            if (userToPermissionConnection == null)
            {
                throw new ObjectNotFoundException("User permission not found.");
            }

            var removedPermission = await _usersPermissionsRepository.RemoveAsync(userToPermissionConnection);
            //return removedPermission.ToUserPermissionReturnDto();
            //return await _userService.GetUserAsync(removedPermission.User.UserName);
        }

        public async Task<List<UserPermissionReturnDto>> DeletePermissionsFromUserAsync(User user,
            List<long> permissionIds)
        {
            var userToPermissionsConnections =
                await (await _usersPermissionsRepository.GetAllAsync(
                        d => d.User == user && permissionIds.Contains(d.Permission.Id),
                        i => i.Permission))
                    .ToListAsync();
            foreach (var item in userToPermissionsConnections)
            {
                if (!permissionIds.Contains(item.Permission.Id))
                    throw new ObjectNotFoundException("Permission not found.");
            }

            await _usersPermissionsRepository.RemoveManyAsync(userToPermissionsConnections);
            return userToPermissionsConnections.Select(e => e.ToUserPermissionReturnDto()).ToList();
        }

        public Task<UserPermissionReturnDto> AddPermissionToUserAsync(UserRoleOrPermissionUpdateDto dto)
        {
            return AddPermissionToUserAsync(dto.UserId, dto.RoleOrPermissionId);
        }

        public Task DeletePermissionFromUserAsync(UserRoleOrPermissionUpdateDto dto)
        {
            return DeletePermissionFromUserAsync(dto.UserId, dto.RoleOrPermissionId);
        }

        public async Task DeleteAllPermissionConnectionsAsync(long permissionId)
        {
            var permissions =
                await _permissionRepository.GetByIdAsync(permissionId);

            if (permissions == null)
            {
                throw new ObjectNotFoundException("Permission not found.");
            }

            var allRoleConnections =
                await (await _usersPermissionsRepository.GetAllAsync(d => d.Permission == permissions)).ToListAsync();

            foreach (var roleConnection in allRoleConnections)
            {
                await _usersPermissionsRepository.RemoveAsync(roleConnection);
            }
        }

        public async Task DeleteAllPermissionFromUserAsync(long userId)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var allPermissionToUserConnections =
                await (await _usersPermissionsRepository.GetAllAsync(d => d.User.Id == userId)).ToListAsync();

            foreach (var userConnection in allPermissionToUserConnections)
            {
                await _usersPermissionsRepository.RemoveAsync(userConnection);
            }
        }

        public async Task DeleteAllPermissionConnectionsAsync(DeleteByIdDto dto)
        {
            await DeleteAllPermissionConnectionsAsync(dto.Id);
        }

        public async Task DeleteAllPermissionFromUserAsync(DeleteByIdDto dto)
        {
            await DeleteAllPermissionFromUserAsync(dto.Id);
        }
    }
}