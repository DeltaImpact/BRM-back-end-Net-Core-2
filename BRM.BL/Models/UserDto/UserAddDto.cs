using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BRM.BL.Models.UserRoleDto;

namespace BRM.BL.Models.UserDto
{
    public class UserAddDto
    {
        [Required] public string Username { get; set; }
        [Required] public ICollection<long> RolesId { get; set; }
        [Required] public ICollection<long> PermissionsId { get; set; }
    }
}