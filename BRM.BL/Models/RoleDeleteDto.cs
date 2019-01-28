using System.ComponentModel.DataAnnotations;

namespace BRM.BL.Models
{
    public class UserAddDto
    {
        [Required] public string Username { get; set; }
    }
}
