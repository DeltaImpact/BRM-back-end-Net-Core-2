using System.Collections.Generic;
using System.Threading.Tasks;
using BRM.BL.Models.UserDto;

namespace BRM.BL.UserService
{
    public interface IUserService
    {
        Task<UserReturnDto> AddUser(string nickname);
        Task<UserReturnDto> GetUser(string nickname);
        Task<List<UserReturnDto>> GetUsers();
    }
}