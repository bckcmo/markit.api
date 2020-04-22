using System.Threading.Tasks;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;
using Markit.Api.Repositories;

namespace Markit.Api.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserEntity> GetById(int id);
        Task<UserEntity> CreateUser(UserRegistration user);
    }
}