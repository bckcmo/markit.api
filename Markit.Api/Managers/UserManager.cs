using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Messages;
using Microsoft.IdentityModel.Tokens;

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
        
        public async Task<User> UpdateUserAsync(User user)
        {
            var userEntity = await _userRepository.Update(user);

            if (userEntity == null)
            {
                return new User();
            }
            
            return new User
            {
                Id = userEntity.Id,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Email = userEntity.Email,
                Reputation = userEntity.Reputation
            };
        }
        
        public async Task<bool> DeleteUserAsync(int id)
        {
            var userEntity = await _userRepository.Delete(id);

            return userEntity != null;
        }

        public async Task<UserAuthResponse> GenerateToken(UserAuth authRequest)
        {
            var user = await _userRepository.GetByEmail(authRequest.Email);
            
            if (user.Password != authRequest.Password)
            {
                throw new Exception("Invalid Password");
            }
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(Environment.GetEnvironmentVariable("JWT_ISSUER"),
                Environment.GetEnvironmentVariable("JWT_ISSUER"),
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            
            return new UserAuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}