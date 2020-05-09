using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Interfaces.Utils;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;
using Markit.Api.Models.Messages;
using Microsoft.IdentityModel.Tokens;

namespace Markit.Api.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordUtil _passwordUtil;

        public UserManager(IUserRepository userRepository, IPasswordUtil passwordUtil)
        {
            _userRepository = userRepository;
            _passwordUtil = passwordUtil;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetById(id);

            return new User
            {
               Id = user.Id,
               FirstName = user.FirstName,
               LastName = user.LastName,
               UserName = user.UserName,
               Reputation = user.Reputation,
            };
        }

        public async Task<User> CreateUserAsync(UserRegistration user)
        {
            user.Password = _passwordUtil.Hash(user.Password);
            var userEntity = await _userRepository.CreateUser(user);
            
            return new User
            {
                Id = userEntity.Id,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                UserName = userEntity.UserName,
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
                UserName = userEntity.UserName,
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
            var user = await _userRepository.GetByUserName(authRequest.UserName);
            
            if (!_passwordUtil.Verify(user.Password, authRequest.Password))
            {
                throw new Exception("Invalid Password");
            }
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
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