using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Interfaces.Managers
{
    public interface IRatingsManager
    {
        Task<IList<UserRating>> QueryByCoordinatesAsync(decimal latitude, decimal longitude);
        Task<RatingsList> GetRatingsByStoreId(int storeId);
        Task<Rating> CreateRating(Rating rating);
        Task<UserRatingsList> GetRecentRatings(int userId);
    }
}