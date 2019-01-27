using System.ComponentModel.DataAnnotations;

namespace BRM.BL.Models.UserDto
{
    public class UserAddDto
    {
        [Required] public string username { get; set; }
    }
}
