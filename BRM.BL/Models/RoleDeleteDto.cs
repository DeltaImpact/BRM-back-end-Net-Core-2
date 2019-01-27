using System.ComponentModel.DataAnnotations;

namespace BRM.BL.Models
{
    public class UserAddDto
    {
        [Required] public string username { get; set; }
    }
}
