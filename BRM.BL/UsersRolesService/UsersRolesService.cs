using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BRM.BL.Exceptions;
using BRM.BL.Extensions.UserRoleExtensions;
using BRM.BL.Models;
using BRM.BL.Models.RoleDto;
using BRM.BL.Models.UserRoleDto;
using BRM.DAO.Entities;
using BRM.DAO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCaching.Internal;
using Microsoft.EntityFrameworkCore;

namespace BRM.BL.UsersRolesService
{
    public class UsersRolesService : IUsersRolesService
    {
        private readonly IRepository<UsersRoles> _usersRolesRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersRolesService(
            IRepository<User> userRepository,
            IRepository<UsersRoles> usersRolesRepository,
            IRepository<Role> roleRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _usersRolesRepository = usersRolesRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserRoleReturnDto> AddRoleToUserAsync(UserRoleOrPermissionUpdateDto dto)
        {
            return await AddRoleToUserAsync(dto.UserId, dto.RoleOrPermissionId);
        }

        public async Task<List<UserRoleReturnDto>> AddRolesToUserAsync(long userId, ICollection<long> rolesId)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var roles = await _roleRepository.GetAllAsync(o => rolesId.Contains(o.Id));
            foreach (var role in roles)
            {
                if (!rolesId.Contains(role.Id)) throw new ObjectNotFoundException("Role not found.");
            }

            var rolesToInsert = roles.Select(role =>
                new UsersRoles
                {
                    Role = role,
                    User = user
                }).ToArray();

            var connections = (await _usersRolesRepository.InsertManyAsync(rolesToInsert))
                .Select(o => o.ToUserRoleReturnDto()).ToList();
            return connections;
        }

        public async Task<List<UserRoleReturnDto>> DeleteRolesFromUserAsync(User user, ICollection<long> rolesId)
        {
            var roles = await _roleRepository.GetAllAsync(o => rolesId.Contains(o.Id));
            foreach (var role in roles)
            {
                if (!rolesId.Contains(role.Id)) throw new ObjectNotFoundException("Role not found.");
            }

            var rolesToRemove = roles.Select(role =>
                new UsersRoles
                {
                    Role = role,
                    User = user
                }).ToArray();

            var disconnections = (await _usersRolesRepository.RemoveManyAsync(rolesToRemove))
                .Select(o => o.ToUserRoleReturnDto()).ToList();
            return disconnections;
        }


        public async Task<UserRoleReturnDto> AddRoleToUserAsync(long userId, long roleOrPermissionId)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var role =
                await _roleRepository.GetByIdAsync(roleOrPermissionId);

            if (role == null)
            {
                throw new ObjectNotFoundException("Role not found.");
            }

            var userToRoleConnection =
                await (await _usersRolesRepository.GetAllAsync(d => d.User == user && d.Role == role))
                    .FirstOrDefaultAsync();

            if (userToRoleConnection != null)
            {
                throw new ObjectNotFoundException("User already have role.");
            }

            var userToRoleForDb = new UsersRoles
            {
                User = user,
                Role = role
            };

            var connection = (await _usersRolesRepository.InsertAsync(userToRoleForDb));
            connection.Role = role;
            connection.User = user;
            return connection.ToUserRoleReturnDto();
            //return await UserService.GetUserAsync(connection.User.UserName);
        }

        public async Task DeleteRoleFromUserAsync(UserRoleOrPermissionUpdateDto dto)
        {
            var user =
                await _userRepository.GetByIdAsync(dto.UserId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var userToRoleConnection =
                await (await _usersRolesRepository.GetAllAsync(d =>
                        d.User == user && d.Role.Id == dto.RoleOrPermissionId))
                    .FirstOrDefaultAsync();

            if (userToRoleConnection == null)
            {
                throw new ObjectNotFoundException("User role not found.");
            }

            await _usersRolesRepository.RemoveAsync(userToRoleConnection);
            //return await UserService.GetUserAsync(removedRole.User.UserName);
        }

        public async Task DeleteAllRoleConnectionsAsync(long roleId)
        {
            var role =
                await _roleRepository.GetByIdAsync(roleId);

            if (role == null)
            {
                throw new ObjectNotFoundException("Role not found.");
            }

            var allRoleConnections =
                await (await _usersRolesRepository.GetAllAsync(d => d.Role == role)).ToListAsync();

            foreach (var roleConnection in allRoleConnections)
            {
                await _usersRolesRepository.RemoveAsync(roleConnection);
            }
        }

        public async Task DeleteAllRoleFromUserAsync(long userId)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var allPermissionToUserConnections =
                await (await _usersRolesRepository.GetAllAsync(d => d.User.Id == userId)).ToListAsync();

            foreach (var userConnection in allPermissionToUserConnections)
            {
                await _usersRolesRepository.RemoveAsync(userConnection);
            }
        }

        public async Task DeleteAllRoleConnectionsAsync(DeleteByIdDto dto)
        {
            await DeleteAllRoleConnectionsAsync(dto.Id);
        }

        public async Task DeleteAllRoleFromUserAsync(DeleteByIdDto dto)
        {
            await DeleteAllRoleFromUserAsync(dto.Id);
        }
    }
}