using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BRM.BL.Models.UserRoleDto
{
    public class UserRoleUpdateDto
    {
        [Required] public long UserId { get; set; }
        [Required] public long RoleId { get; set; }
    }
}