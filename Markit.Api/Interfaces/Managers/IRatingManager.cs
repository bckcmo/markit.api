using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Interfaces.Managers
{
    public interface IRatingsManager
    {
        Task<IList<Rating>> QueryByCoordinatesAsync(decimal latitude, decimal longitude);
        Task<Rating> CreateRating(Rating rating);
    }
}