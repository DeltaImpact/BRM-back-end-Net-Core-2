using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BRM.BL.Exceptions;
using BRM.BL.Extensions.PermissionDtoExtensions;
using BRM.BL.Extensions.RoleDtoExtensions;
using BRM.BL.Models;
using BRM.BL.Models.PermissionDto;
using BRM.BL.Models.RoleDto;
using BRM.BL.UsersPermissionsService;
using BRM.BL.UsersRolesService;
using BRM.BL.UserService;
using BRM.DAO.Entities;
using BRM.DAO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BRM.BL.PermissionsService
{
    public class PermissionsService : IPermissionsService
    {
        private readonly IUsersPermissionsService _usersPermissionsService;
        private readonly IRepository<Permission> _permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionsService(
            IUsersPermissionsService usersPermissionsService,
            IRepository<Permission> permissionService, IHttpContextAccessor httpContextAccessor)
        {
            _usersPermissionsService = usersPermissionsService;
            _permissionService = permissionService;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<PermissionReturnDto> AddPermission(PermissionAddDto dto)
        {
            var roleInDb =
                await (await _permissionService.GetAllAsync(d => d.Name == dto.PermissionName))
                    .FirstOrDefaultAsync();
            if (roleInDb != null)
            {
                throw new ObjectAlreadyExistException("Permission with such name already added.");
            }

            var userForDb = new Permission
            {
                Name = dto.PermissionName,
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

        public async Task<PermissionReturnDto> DeletePermission(long permissionId)
        {
            var role =
                await _permissionService.GetByIdAsync(permissionId);

            if (role == null)
            {
                throw new ObjectNotFoundException("Role not found.");
            }

            await _usersPermissionsService.DeleteAllPermissionConnections(role.Id);

            var removedPin = await _permissionService.RemoveAsync(role);
            return removedPin.ToPermissionReturnDto();
        }

        public async Task<PermissionReturnDto> DeletePermission(DeleteByIdDto dto)
        {
            return await DeletePermission(dto.Id);
        }

        public async Task<PermissionReturnDto> UpdatePermissionAsync(PermissionUpdateDto model)
        {
            var permissionOld =
                await _permissionService.GetByIdAsync(model.Id);
            if (permissionOld == null)
            {
                throw new ObjectNotFoundException("Permission not found.");
            }

            var permission =
                await _permissionService.UpdateAsync(model.ToPermission());
            return permission.ToPermissionReturnDto();
        }
    }
}