using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BRM.BL.Exceptions;
using BRM.BL.Extensions.RoleDtoExtensions;
using BRM.BL.Models;
using BRM.BL.Models.RoleDto;
using BRM.BL.UsersRolesService;
using BRM.DAO.Entities;
using BRM.DAO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BRM.BL.RolesService
{
    public class RolesService : IRolesService
    {
        private readonly IRepository<UsersRoles> _usersRolesRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IUsersRolesService _usersRolesService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RolesService(
            IRepository<User> userRepository,
            IRepository<UsersRoles> usersRolesRepository,
            IRepository<Role> roleRepository,
            IUsersRolesService usersRolesService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _usersRolesRepository = usersRolesRepository;
            _roleRepository = roleRepository;
            _usersRolesService = usersRolesService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<RoleReturnDto> AddRole(RoleAddDto dto)
        {
            var roleInDb =
                await (await _roleRepository.GetAllAsync(d => d.Name == dto.RoleName))
                    .FirstOrDefaultAsync();
            if (roleInDb != null)
            {
                throw new ObjectAlreadyExistException("UserRole with such name already added.");
            }

            var userForDb = new Role
            {
                Name = dto.RoleName,
            };

            var user = (await _roleRepository.InsertAsync(userForDb));

            return user.ToRoleReturnDto();

            //throw new NotImplementedException();
        }

        public async Task<List<RoleReturnDto>> GetRoles()
        {
            var roles =
                (await _roleRepository.GetAllAsync())
                .Select(e => e.ToRoleReturnDto())
                .ToList();

            return roles;
        }

        public async Task<RoleReturnDto> DeleteRole(DeleteByIdDto dto)
        {
            return await DeleteRole(dto.Id);
        }

        public async Task<RoleReturnDto> UpdateRoleAsync(RoleUpdateDto model)
        {
            var roleOld =
                await _roleRepository.GetByIdAsync(model.Id);
            if (roleOld == null)
            {
                throw new ObjectNotFoundException("Role not found.");
            }

            var role =
                await _roleRepository.UpdateAsync(model.ToRole(roleOld));
            return role.ToRoleReturnDto();
        }

        public async Task<RoleReturnDto> DeleteRole(long roleId)
        {
            var role =
                await _roleRepository.GetByIdAsync(roleId);

            if (role == null)
            {
                throw new ObjectNotFoundException("Role not found.");
            }

            await _usersRolesService.DeleteAllRoleConnections(role.Id);

            var removedRole = await _roleRepository.RemoveAsync(role);
            return removedRole.ToRoleReturnDto();
        }

        public async Task DeleteAllRolesFromUser(long userId)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            var allRoleToUserConnections =
                await (await _usersRolesRepository.GetAllAsync(d => d.User.Id == userId)).ToListAsync();

            foreach (var userConnection in allRoleToUserConnections)
            {
                await _usersRolesRepository.RemoveAsync(userConnection);
            }
        }
    }
}