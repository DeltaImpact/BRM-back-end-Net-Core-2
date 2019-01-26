using System.Collections.Generic;
using BRM.BL.Models.PermissionDto;

namespace BRM.BL.Models.UserDto
{
    public class UserReturnDto
    {
        public string UserName { get; set; }
        public long Id { get; set; }
        public List<PermissionReturnDto> Permissions { get; set; }
        public List<PermissionReturnDto> Roles { get; set; }
    }
}
