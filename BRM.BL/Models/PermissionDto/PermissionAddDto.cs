using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BRM.BL.Models.RoleDto
{
    public class PermissionAddDto
    {
        [Required] public string permissionName { get; set; }
    }
}
