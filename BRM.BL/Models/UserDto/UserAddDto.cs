using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BRM.BL.Models.RoleDto
{
    public class UserAddDto
    {
        [Required] public string username { get; set; }
    }
}
