using System.ComponentModel.DataAnnotations;

namespace BRM.BL.Models.PermissionDto
{
    public class PermissionUpdateDto
    {
        [Required] public string Name { get; set; }
        [Required] public long Id { get; set; }
    }
}