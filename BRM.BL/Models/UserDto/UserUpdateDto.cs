using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BRM.BL.Models.UserDto
{
    public class UserUpdateDto
    {
        [Required] public long Id { get; set; }
        [Required] public string UserName { get; set; }
        [Required] public ICollection<long> RolesId { get; set; }
        [Required] public ICollection<long> PermissionsId { get; set; }
    }
}