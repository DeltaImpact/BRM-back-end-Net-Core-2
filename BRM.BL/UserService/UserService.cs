using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BRM.BL.Exceptions;
using BRM.BL.Extensions.PermissionDtoExtensions;
using BRM.BL.Extensions.RoleDtoExtensions;
using BRM.BL.Extensions.UserDtoExtensions;
using BRM.BL.Models;
using BRM.BL.Models.UserDto;
using BRM.BL.Models.UserPermissionDto;
using BRM.BL.Models.UserRoleDto;
using BRM.BL.UsersPermissionsService;
using BRM.BL.UsersRolesService;
using BRM.DAO.Entities;
using BRM.DAO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UserAddDto = BRM.BL.Models.UserDto.UserAddDto;

namespace BRM.BL.UserService
{
    public class UserService : IUserService
    {
        private readonly IUsersRolesService _usersRolesService;
        private readonly IUsersPermissionsService _usersPermissionsService;
        private readonly IRepository<UsersRoles> _usersRolesRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UsersPermissions> _usersPermissionsRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            IUsersRolesService usersRolesService,
            IUsersPermissionsService usersPermissionsService,
            IRepository<User> userRepository,
            IRepository<UsersPermissions> usersPermissionsRepository,
            IRepository<UsersRoles> usersRolesRepository,
            IRepository<Role> roleRepository,
            IHttpContextAccessor httpContextAccessor)

        {
            _usersRolesService = usersRolesService;
            _usersPermissionsService = usersPermissionsService;
            _usersRolesRepository = usersRolesRepository;
            _userRepository = userRepository;
            _usersPermissionsRepository = usersPermissionsRepository;
            _roleRepository = roleRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<UserReturnDto> AddUserAsync(UserAddDto dto)
        {
            return AddUserAsync(dto.Username);
        }

        public async Task<UserReturnDto> AddUserAsync(string nickname)
        {
            var userInDb =
                await (await _userRepository.GetAllAsync(d => d.UserName == nickname))
                    .FirstOrDefaultAsync();
            if (userInDb != null)
            {
                throw new ObjectAlreadyExistException("User with such name already added.");
            }

            var userForDb = new User
            {
                UserName = nickname,
            };


            var user = (await _userRepository.InsertAsync(userForDb)).ToUserReturnDto();

            return user;

            //throw new NotImplementedException();
        }

        public Task<UserReturnDto> GetUserAsync(UserAddDto dto)
        {
            return GetUserAsync(dto.Username);
        }

        public async Task<UserReturnDto> GetUserByIdAsync(long id)
        {
            var userInDb =
                await _userRepository.GetByIdAsync(id);
            if (userInDb == null)
            {
                throw new ObjectAlreadyExistException("User with such name not exist.");
            }

            var permissions =
                (await _usersPermissionsRepository.GetAllAsync(d => d.User == userInDb, x => x.Permission))
                .Select(e => e.Permission.ToPermissionReturnDto())
                .ToListAsync();

            var roles =
                (await _usersRolesRepository.GetAllAsync(d => d.User == userInDb, x => x.Role))
                .Select(e => e.Role.ToRoleReturnDto())
                .ToListAsync();

            return userInDb.ToUserReturnDto(await permissions, await roles);
        }

        public async Task<UserReturnDto> GetUserAsync(string nickname)
        {
            var userInDb =
                await (await _userRepository.GetAllAsync(d => d.UserName == nickname))
                    .FirstOrDefaultAsync();
            if (userInDb == null)
            {
                throw new ObjectAlreadyExistException("User with such name not exist.");
            }

            var permissions =
                (await _usersPermissionsRepository.GetAllAsync(d => d.User == userInDb, x => x.Permission))
                .Select(e => e.Permission.ToPermissionReturnDto())
                .ToListAsync();

            var roles =
                (await _usersRolesRepository.GetAllAsync(d => d.User == userInDb, x => x.Role))
                .Select(e => e.Role.ToRoleReturnDto())
                .ToListAsync();

            return userInDb.ToUserReturnDto(await permissions, await roles);
        }

        public async Task<List<UserReturnDto>> GetUsersAsync()
        {
            var users =
                (await _userRepository.GetAllAsync())
                .Include(e => e.Permissions)
                .ThenInclude(e => e.Permission)
                .Include(e => e.Roles)
                .ThenInclude(e => e.Role)
                .Select(e => e.ToUserReturnDto(e.Permissions.Select(x => x.Permission.ToPermissionReturnDto()).ToList(),
                    e.Roles.Select(z => z.Role.ToRoleReturnDto()).ToList()))
                .ToList();

            return users;
        }

        public async Task DeleteUserAsync(DeleteByIdDto dto)
        {
            await DeleteUserAsync(dto.Id);
        }

        public async Task DeleteUserAsync(long userId)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }


            await _usersRolesService.DeleteAllRoleFromUserAsync(user.Id);
            await _usersPermissionsService.DeleteAllPermissionFromUserAsync(user.Id);

            await _userRepository.RemoveAsync(user);
        }

        public async Task<UserReturnDto> UpdateUserAsync(UserUpdateDto model)
        {
            var userOld =
                await (await _userRepository.GetAllAsync(u => u.Id == model.Id))
                    .Include(e => e.Permissions)
                    .ThenInclude(e => e.Permission)
                    .Include(e => e.Roles)
                    .ThenInclude(e => e.Role)
                    .FirstAsync();

            if (userOld == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            if ((await _userRepository.GetAllAsync(x => x.UserName == model.UserName && x.Id != model.Id)).Any())
                throw new ObjectNotFoundException("User with same nickname already exist.");
            userOld.UserName = model.UserName;

            userOld.Permissions = await DeleteExcessPermissionsFromUser(userOld, model.PermissionsIds);
            userOld.Roles = await DeleteExcessRolesFromUser(userOld, model.RolesIds);

            var user =
                await _userRepository.UpdateAsync(userOld);
            var userReturnDto = user.ToUserReturnDto();

            if (userOld.Permissions.Count == 0)
            {
                userReturnDto.Permissions =
                    (await AddMissingPermissionsToUser(userOld, model.PermissionsIds)).Select(e => e.Permission)
                    .ToList();
            }
            else
            {
                userReturnDto.Permissions.AddRange(
                    (await AddMissingPermissionsToUser(userOld, model.PermissionsIds)).Select(e => e.Permission));
            }

            if (userOld.Roles.Count == 0)
            {
                userReturnDto.Roles =
                    (await AddMissingRolesToUser(userOld, model.RolesIds)).Select(e => e.Role).ToList();
            }
            else
            {
                userReturnDto.Roles.AddRange(
                    (await AddMissingRolesToUser(userOld, model.RolesIds)).Select(e => e.Role));
            }


            return userReturnDto;
        }

        private async Task<List<UserPermissionReturnDto>> AddMissingPermissionsToUser(User user,
            IEnumerable<long> permissionsShouldBeIds)
        {
            var newPermissionsConnections = new List<UserPermissionReturnDto>();
            var permissionsToAddIds =
                permissionsShouldBeIds.Except(user.Permissions.Select(e => e.Permission.Id).ToArray()).ToList();
            if (permissionsToAddIds.Count != 0)
            {
                newPermissionsConnections =
                    await _usersPermissionsService.AddPermissionsToUserAsync(user.Id, permissionsToAddIds);
            }

            return newPermissionsConnections;
        }

        private async Task<List<UserRoleReturnDto>> AddMissingRolesToUser(User user,
            IEnumerable<long> rolesShouldBeIds)
        {
            var newRolesConnections = new List<UserRoleReturnDto>();
            var rolesToAddIds =
                rolesShouldBeIds.Except(user.Roles.Select(e => e.Role.Id).ToArray()).ToList();
            if (rolesToAddIds.Count != 0)
            {
                newRolesConnections =
                    await _usersRolesService.AddRolesToUserAsync(user.Id, rolesToAddIds);
            }

            return newRolesConnections;
        }

        private async Task<List<UsersPermissions>> DeleteExcessPermissionsFromUser(User user,
            IEnumerable<long> permissionsShouldBeIds)
        {
            var permissionsToDeleteIds =
                user.Permissions.Select(e => e.Permission.Id).ToArray().Except(permissionsShouldBeIds).ToList();
            if (permissionsToDeleteIds.Count == 0) return new List<UsersPermissions>();

            await _usersPermissionsService.DeletePermissionsFromUserAsync(user, permissionsToDeleteIds);
            return user.Permissions.Where(e => !permissionsToDeleteIds.Contains(e.Id)).ToList();
        }

        private async Task<List<UsersRoles>> DeleteExcessRolesFromUser(User user,
            IEnumerable<long> rolesShouldBeIds)
        {
            var rolesToDeleteIds =
                user.Roles.Select(e => e.Role.Id).ToArray().Except(rolesShouldBeIds).ToList();
            if (rolesToDeleteIds.Count == 0) return new List<UsersRoles>();

            await _usersRolesService.DeleteRolesFromUserAsync(user, rolesToDeleteIds);
            return user.Roles.Where(e => !rolesToDeleteIds.Contains(e.Id)).ToList();
        }
    }
}