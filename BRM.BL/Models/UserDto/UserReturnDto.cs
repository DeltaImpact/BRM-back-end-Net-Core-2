using System.Collections.Generic;
using BRM.BL.Models.PermissionDto;
using BRM.BL.Models.RoleDto;

namespace BRM.BL.Models.UserDto
{
    public class UserReturnDto
    {
        public string UserName { get; set; }
        public long Id { get; set; }
        public List<PermissionReturnDto> Permissions { get; set; }
        public List<RoleReturnDto> Roles { get; set; }
    }
}
