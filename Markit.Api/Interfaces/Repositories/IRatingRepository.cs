using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;

namespace Markit.Api.Interfaces.Repositories
{
    public interface IRatingRepository
    {
        Task<List<RatingEntity>> GetRatingsForStoresAsync(IEnumerable<int> storeIds);
        Task<RatingEntity> CreateAsync(Rating rating);
    }
}