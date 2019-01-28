using BRM.BL.Models.UserDto;
using BRM.DAO.Entities;

namespace BRM.BL.Extensions.UserDtoExtensions
{
    public static class UserUpdateDtoExtensions
    {
        public static Role ToUser(this UserUpdateDto model)
        {
            var role = new Role
            {
                Name = model.Name,
                Id = model.Id
            };
            return role;
        }
    }
}
