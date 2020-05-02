using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Managers
{
    public class RatingsManager : IRatingsManager
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IStoreRepository _storeRepository;

        public RatingsManager(IRatingRepository ratingRepository, IStoreRepository storeRepository)
        {
            _ratingRepository = ratingRepository;
            _storeRepository = storeRepository;
        }

        public async Task<Rating> CreateRating(Rating rating)
        {
            var store = await _storeRepository.GetStoreById(rating.Store.Id);
            var newRating = await _ratingRepository.CreateAsync(rating);
            
            return new Rating
            {
                Id = newRating.Id,
                Comment = rating.Comment,
                Points = rating.Points,
                UserId = rating.UserId,
                Store = new Store
                {
                    StreetAddress = store.StreetAddress,
                    City = store.City,
                    State = store.State,
                    PostalCode = store.PostalCode,
                    Id = store.Id,
                    Name = store.Name,
                    Coordinate = new Coordinate
                    {
                        Latitude = store.Latitude,
                        Longitude = store.Longitude
                    }
                }
            };
        }
        
        public async Task<IList<Rating>> QueryByCoordinatesAsync(decimal latitude, decimal longitude)
        {
            var stores = await _storeRepository.QueryByCoordinates(latitude, longitude);
            var storeIds = stores.Select(s => s.Id);
            var ratingEntities = await _ratingRepository.GetRatingsForStoresAsync(storeIds);
            var ratings = ratingEntities.Join(stores, r
                => r.StoreId, s
                => s.Id, (rating, store) => new Rating
            {
                Id = rating.Id,
                UserId = rating.UserId,
                Comment = rating.Comment,
                Points = rating.Points,
                Store = new Store
                {
                    Id = store.Id,
                    Name = store.Name,
                    StreetAddress = store.StreetAddress,
                    City = store.City,
                    State = store.State,
                    PostalCode = store.PostalCode,
                    Coordinate = new Coordinate
                    {
                        Latitude = store.Latitude,
                        Longitude = store.Longitude
                    }
                }
            });
            
            return ratings.ToList();
        }
    }
}