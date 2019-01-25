using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackSide2.BL.Exceptions;
using BRM.BL.Extensions.PermissionDtoExtensions;
using BRM.BL.Extensions.RoleDtoExtensions;
using BRM.BL.Extensions.UserDtoExtensions;
using BRM.BL.Models.UserDto;
using BRM.BL.Models.UserRoleDto;
using BRM.BL.PermissionsService;
using BRM.BL.UserService;
using BRM.DAO.Entities;
using BRM.DAO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BRM.BL.RolesService
{
    public class PermissionsService : IPermissionsService
    {
        private readonly IRepository<UsersRoles> _usersRoles;
        private readonly IRepository<User> _user;
        private readonly IRepository<UsersPermissions> _usersPermissions;
        private readonly IRepository<Role> _roleService;
        private readonly IRepository<Permission> _permissionService;
        private readonly IUsersRolesService _usersRolesService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionsService(
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


        public async Task<PermissionReturnDto> AddPermission(string permissionName)
        {
            var roleInDb =
                await (await _permissionService.GetAllAsync(d => d.Name == permissionName))
                    .FirstOrDefaultAsync();
            if (roleInDb != null)
            {
                throw new ObjectAlreadyExistException("Permission with such name already added.");
            }

            var userForDb = new Permission
            {
                Name = permissionName,
            };

            var user = (await _permissionService.InsertAsync(userForDb));

            return user.ToPermissionReturnDto();
        }

        public async Task<List<PermissionReturnDto>> GetPermissions()
        {
            var items =
                (await _permissionService.GetAllAsync())
                .Select(e => e.ToPermissionReturnDto())
                .ToList();

            return items;
        }
    }
}