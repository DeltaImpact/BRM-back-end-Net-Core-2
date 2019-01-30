using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BRM.BL.Exceptions;
using BRM.BL.Extensions.PermissionDtoExtensions;
using BRM.BL.Extensions.RoleDtoExtensions;
using BRM.BL.Extensions.UserDtoExtensions;
using BRM.BL.Models;
using BRM.BL.Models.UserDto;
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

        public Task<UserReturnDto> AddUser(UserAddDto dto)
        {
            return AddUser(dto.Username);
        }

        public async Task<UserReturnDto> AddUser(string nickname)
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

        public Task<UserReturnDto> GetUser(UserAddDto dto)
        {
            return GetUser(dto.Username);
        }

        public async Task<UserReturnDto> GetUserById(long id)
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

        public async Task<UserReturnDto> GetUser(string nickname)
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

        public async Task<List<UserReturnDto>> GetUsers()
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

        public async Task DeleteUser(DeleteByIdDto dto)
        {
            await DeleteUser(dto.Id);
        }

        public async Task DeleteUser(long userId)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }


            await _usersRolesService.DeleteAllRoleFromUser(user.Id);
            await _usersPermissionsService.DeleteAllPermissionFromUser(user.Id);

            await _userRepository.RemoveAsync(user);
        }

        public async Task<UserReturnDto> UpdateUserAsync(UserUpdateDto model)
        {
            var userOld =
                await _userRepository.GetByIdAsync(model.Id);
            if (userOld == null)
            {
                throw new ObjectNotFoundException("User not found.");
            }

            if ((await _userRepository.GetAllAsync(x => x.UserName == model.Name)).Any())
                throw new ObjectNotFoundException("User with same nickname already exist.");

            userOld.UserName = model.Name;

            var user =
                await _userRepository.UpdateAsync(userOld);
            return user.ToUserReturnDto();
        }
    }
}