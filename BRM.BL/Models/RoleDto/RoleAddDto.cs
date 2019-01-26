using System.ComponentModel.DataAnnotations;

namespace BRM.BL.Models.RoleDto
{
    public class RoleAddDto
    {
        [Required] public string RoleName { get; set; }
    }
}
