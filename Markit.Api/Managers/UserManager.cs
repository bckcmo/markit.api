using System.Threading.Tasks;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Managers
{
    public class UserManager : IUserManager
    {
        private IUserRepository _userRepository;
        
        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetById(id);
            
            return new User
            {
               Id = user.Id,
               FirstName = user.FirstName,
               LastName = user.LastName,
               Email = user.Email,
               Reputation = user.Reputation,
            };
        }

        public async Task<User> CreateUserAsync(UserRegistration user)
        {
            var userEntity = await _userRepository.CreateUser(user);
            
            return new User
            {
                Id = userEntity.Id,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Email = userEntity.Email,
                Reputation = userEntity.Reputation
            };
        }
    }
}