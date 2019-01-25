using BRM.BL.Models.UserDto;

namespace BRM.BL.Models.UserRoleDto
{
    public class UserRoleReturnDto
    {
        public UserReturnDto User { get; set; }
        public PermissionReturnDto Role { get; set; }
    }
}
