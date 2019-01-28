using System.ComponentModel.DataAnnotations;

namespace BRM.BL.Models.PermissionDto
{
    public class PermissionAddDto
    {
        [Required] public string PermissionName { get; set; }
    }
}
