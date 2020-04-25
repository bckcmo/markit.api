using System.Threading.Tasks;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Messages;

namespace Markit.Api.Interfaces.Managers
{
    public interface IUserManager
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(UserRegistration user);
        Task<UserAuthResponse> GenerateToken(UserAuth authRequest);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
    }
}