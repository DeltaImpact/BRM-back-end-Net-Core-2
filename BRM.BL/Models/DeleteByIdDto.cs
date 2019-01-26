using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BRM.BL.Models.RoleDto
{
    public class DeleteByIdDto
    {
        [Required] public int Id { get; set; }
    }
}
