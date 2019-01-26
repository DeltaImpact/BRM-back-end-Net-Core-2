using System.ComponentModel.DataAnnotations;

namespace BRM.BL.Models.UserRoleDto
{
    public class UserRoleOrPermissionUpdateDto
    {
        [Required] public long UserId { get; set; }
        [Required] public long RoleOrPermissionId { get; set; }
    }
}