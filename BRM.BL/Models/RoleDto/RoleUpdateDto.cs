using System.ComponentModel.DataAnnotations;

namespace BRM.BL.Models.RoleDto
{
    public class RoleUpdateDto
    {
        [Required] public string Name { get; set; }
        [Required] public long Id { get; set; }
    }
}