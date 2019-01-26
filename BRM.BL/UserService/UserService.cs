using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BRM.BL.Exceptions;
using BRM.BL.Extensions.PermissionDtoExtensions;
using BRM.BL.Extensions.RoleDtoExtensions;
using BRM.BL.Extensions.UserDtoExtensions;
using BRM.BL.Models.RoleDto;
using BRM.BL.Models.UserDto;
using BRM.DAO.Entities;
using BRM.DAO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BRM.BL.UserService
{
    public class UserService : IUserService
    {
        private readonly IRepository<UsersRoles> _usersRoles;
        private readonly IRepository<User> _userService;
        private readonly IRepository<UsersPermissions> _usersPermissions;
        private readonly IRepository<Role> _roleService;
        private readonly IRepository<Permission> _permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
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

        public Task<UserReturnDto> AddUser(UserAddDto dto)
        {
            return AddUser(dto.username);
        }

        public async Task<UserReturnDto> AddUser(string nickname)
        {
            var userInDb =
                await (await _userService.GetAllAsync(d => d.UserName == nickname))
                    .FirstOrDefaultAsync();
            if (userInDb != null)
            {
                throw new ObjectAlreadyExistException("User with such name already added.");
            }

            var userForDb = new User
            {
                UserName = nickname,
            };


            var user = (await _userService.InsertAsync(userForDb)).ToUserReturnDto();

            return user;

            //throw new NotImplementedException();
        }

        public Task<UserReturnDto> GetUser(UserAddDto dto)
        {
            return GetUser(dto.username);
        }

        public async Task<UserReturnDto> GetUser(string nickname)
        {
            var userInDb =
                await (await _userService.GetAllAsync(d => d.UserName == nickname))
                    .FirstOrDefaultAsync();
            if (userInDb == null)
            {
                throw new ObjectAlreadyExistException("User with such name not exist.");
            }

            var permissions =
                (await _usersPermissions.GetAllAsync(d => d.User == userInDb, x => x.Permission))
                .Select(e => e.Permission.ToPermissionReturnDto())
                .ToListAsync();

            var roles =
                (await _usersRoles.GetAllAsync(d => d.User == userInDb, x => x.Role))
                .Select(e => e.Role.ToRoleReturnDto())
                .ToListAsync();

            return userInDb.ToUserReturnDto(await permissions, await roles);
        }

        public async Task<List<UserReturnDto>> GetUsers()
        {
            var users =
                (await _userService.GetAllAsync())
                .Include(e => e.Permissions)
                .ThenInclude(e => e.Permission)
                .Include(e => e.Roles)
                .ThenInclude(e => e.Role)
                //.Select(e => new
                //{
                //    User = e,
                //    Permissions = e.Permissions.ToList(),
                //    Roles = e.Roles.ToList(),
                //    Usr = e.ToUserReturnDto(e.Permissions.ToList(), e.Roles.ToList(),)
                //})
                .Select(e => e.ToUserReturnDto(e.Permissions.Select(x => x.Permission.ToPermissionReturnDto()).ToList(),
                    e.Roles.Select(z => z.Role.ToRoleReturnDto()).ToList()))
                .ToList();

            //var usersForReturning = new List<UserReturnDto>();
            //foreach (var user in users)
            //{
            //    usersForReturning.Add(user.User.ToUserReturnDto(user.Permissions.Select(e => e.Permission).ToList(),
            //        user.Roles.Select(e => e.UserRole).ToList()));
            //}

            return users;
            //throw new NotImplementedException();
        }
    }
}