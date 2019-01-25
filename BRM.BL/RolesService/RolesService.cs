using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackSide2.BL.Exceptions;
using BRM.BL.Extensions.RoleDtoExtensions;
using BRM.BL.Extensions.UserDtoExtensions;
using BRM.BL.Models.UserDto;
using BRM.BL.Models.UserRoleDto;
using BRM.BL.UserService;
using BRM.DAO.Entities;
using BRM.DAO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BRM.BL.RolesService
{
    public class RolesService : IRolesService
    {
        private readonly IRepository<UsersRoles> _usersRoles;
        private readonly IRepository<User> _user;
        private readonly IRepository<UsersPermissions> _usersPermissions;
        private readonly IRepository<Role> _roleService;
        private readonly IRepository<Permission> _permissionService;
        private readonly IUsersRolesService _usersRolesService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RolesService(
            IRepository<User> user,
            IRepository<UsersPermissions> usersPermissions,
            IRepository<UsersRoles> usersRoles,
            IRepository<Role> roleService,
            IRepository<Permission> permissionService,
            IUsersRolesService usersRolesService,
            IHttpContextAccessor httpContextAccessor)
        {
            _user = user;
            _usersPermissions = usersPermissions;
            _usersRoles = usersRoles;
            _roleService = roleService;
            _permissionService = permissionService;
            _usersRolesService = usersRolesService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PermissionReturnDto> AddRole(string roleName)
        {
            var roleInDb =
                await (await _roleService.GetAllAsync(d => d.Name == roleName))
                    .FirstOrDefaultAsync();
            if (roleInDb != null)
            {
                throw new ObjectAlreadyExistException("UserRole with such name already added.");
            }

            var userForDb = new Role
            {
                Name = roleName,
            };

            var user = (await _roleService.InsertAsync(userForDb));

            return user.ToRoleReturnDto();

            //throw new NotImplementedException();
        }

        public async Task<List<PermissionReturnDto>> GetRoles()
        {
            var roles =
                (await _roleService.GetAllAsync())
                .Select(e => e.ToRoleReturnDto())
                .ToList();

            return roles;
        }

        public async Task<PermissionReturnDto> DeleteRole(long roleId)
        {
            var role =
                await _roleService.GetByIdAsync(roleId);

            if (role == null)
            {
                throw new ObjectNotFoundException("UserRole not found.");
            }

            await _usersRolesService.DeleteAllRoleConnections(role.Id);

            var removedPin = await _roleService.RemoveAsync(role);
            return removedPin.ToRoleReturnDto();
            //throw new System.NotImplementedException();
        }
    }
}