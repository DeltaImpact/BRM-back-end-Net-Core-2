using System.ComponentModel.DataAnnotations;

namespace BRM.BL.Models.UserDto
{
    public class UserAddDto
    {
        [Required] public string Username { get; set; }
    }
}
