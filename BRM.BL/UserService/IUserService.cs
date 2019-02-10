using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models;
using BRM.BL.Models.UserDto;
using UserAddDto = BRM.BL.Models.UserDto.UserAddDto;

namespace BRM.BL.UserService
{
    public interface IUserService
    {
        Task<UserReturnDto> AddUserAsync(UserAddDto dto);
        Task<UserReturnDto> AddUserAsync(string nickname);
        Task<UserReturnDto> GetUserAsync(UserAddDto dto);
        Task<UserReturnDto> GetUserByIdAsync(long id);
        Task<UserReturnDto> GetUserAsync(string nickname);
        Task<List<UserReturnDto>> GetUsersAsync();
        Task DeleteUserAsync(long userId);
        Task DeleteUserAsync(DeleteByIdDto dto);
        Task<UserReturnDto> UpdateUserAsync(UserUpdateDto model);
    }
}