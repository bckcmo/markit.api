using System.Threading.Tasks;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Interfaces.Managers
{
    public interface IUserManager
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(UserRegistration user);
    }
}