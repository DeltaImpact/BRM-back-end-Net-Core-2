using System.Threading.Tasks;
using BRM.BL.Exceptions;
using BRM.BL.Extensions.UserRoleExtensions;
using BRM.BL.Models;
using BRM.BL.Models.UserRoleDto;
using BRM.DAO.Entities;
using BRM.DAO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BRM.BL.UsersRolesService
{
    public class UsersRolesService : IUsersRolesService
    {
        private readonly IRepository<UsersRoles> _usersRoles;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersRolesService(
            IRepository<User> userRepository,
            IRepository<UsersRoles> usersRoles,
            IRepository<Role> roleService, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _usersRoles = usersRoles;
            _userRepository = userRepository;
            _roleService = roleService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserRoleReturnDto> AddRoleToUser(UserRoleOrPermissionUpdateDto dto)
        {
            var user =
                await _userRepository.GetByIdAsync(dto.UserId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var role =
                await _roleService.GetByIdAsync(dto.RoleOrPermissionId);

            if (role == null)
            {
                throw new ObjectNotFoundException("Role not found.");
            }

            var userToRoleConnection =
                await (await _usersRoles.GetAllAsync(d => d.User == user && d.Role == role))
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

            var connection = (await _usersRoles.InsertAsync(userToRoleForDb));
            return connection.ToUserRoleReturnDto();
            //return await UserService.GetUser(connection.User.UserName);
        }

        public async Task DeleteRoleFromUser(UserRoleOrPermissionUpdateDto dto)
        {
            var user =
                await _userRepository.GetByIdAsync(dto.UserId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var userToRoleConnection =
                await (await _usersRoles.GetAllAsync(d => d.User == user && d.Role.Id == dto.RoleOrPermissionId))
                    .FirstOrDefaultAsync();

            if (userToRoleConnection == null)
            {
                throw new ObjectNotFoundException("User role not found.");
            }

            await _usersRoles.RemoveAsync(userToRoleConnection);
            //return await UserService.GetUser(removedRole.User.UserName);
        }

        public async Task DeleteAllRoleConnections(long roleId)
        {
            var role =
                await _roleService.GetByIdAsync(roleId);

            if (role == null)
            {
                throw new ObjectNotFoundException("Role not found.");
            }

            var allRoleConnections =
                await (await _usersRoles.GetAllAsync(d => d.Role == role)).ToListAsync();

            foreach (var roleConnection in allRoleConnections)
            {
                await _usersRoles.RemoveAsync(roleConnection);
            }
        }

        public async Task DeleteAllRoleFromUser(long userId)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var allPermissionToUserConnections =
                await(await _usersRoles.GetAllAsync(d => d.User.Id == userId)).ToListAsync();

            foreach (var userConnection in allPermissionToUserConnections)
            {
                await _usersRoles.RemoveAsync(userConnection);
            }
        }

        public async Task DeleteAllRoleConnections(DeleteByIdDto dto)
        {
            await DeleteAllRoleConnections(dto.Id);
        }

        public async Task DeleteAllRoleFromUser(DeleteByIdDto dto)
        {
            await DeleteAllRoleFromUser(dto.Id);
        }
    }
}