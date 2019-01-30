using BRM.BL.Models.RoleDto;
using BRM.BL.Models.UserDto;

namespace BRM.BL.Models.UserRoleDto
{
    public class UserRoleReturnDto
    {
        public UserReturnDto User { get; set; }
        public RoleReturnDto Role { get; set; }
    }
}
