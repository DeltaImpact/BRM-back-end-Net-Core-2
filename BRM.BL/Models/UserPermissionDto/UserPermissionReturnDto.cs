using BRM.BL.Models.UserDto;

namespace BRM.BL.Models.UserPermissionDto
{
    public class UserPermissionReturnDto
    {
        public UserReturnDto User { get; set; }
        public PermissionReturnDto Permission { get; set; }
    }
}
