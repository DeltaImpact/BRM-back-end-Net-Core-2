using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BRM.BL.Models.UserRoleDto;

namespace BRM.BL.Models.UserDto
{
    public class UserAddDto
    {
        [Required] public string Username { get; set; }
        public ICollection<long> RolesId { get; set; }
        public ICollection<long> PermissionsId { get; set; }
    }
}
