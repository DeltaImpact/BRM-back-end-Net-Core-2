using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models;
using BRM.BL.Models.RoleDto;
using BRM.BL.Models.UserDto;
using UserAddDto = BRM.BL.Models.UserDto.UserAddDto;

namespace BRM.BL.UserService
{
    public interface IUserService
    {
        Task<UserReturnDto> AddUser(UserAddDto dto);
        Task<UserReturnDto> AddUser(string nickname);
        Task<UserReturnDto> GetUser(UserAddDto dto);
        Task<UserReturnDto> GetUser(string nickname);
        Task<List<UserReturnDto>> GetUsers();
        Task DeleteUser(long userId);
        Task DeleteUser(DeleteByIdDto dto);
        Task<UserReturnDto> UpdateUserAsync(UserUpdateDto model);
    }
}