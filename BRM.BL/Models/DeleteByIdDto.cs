using System.ComponentModel.DataAnnotations;

namespace BRM.BL.Models
{
    public class DeleteByIdDto
    {
        [Required] public int Id { get; set; }
    }
}
