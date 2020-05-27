using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Managers
{
    public class RatingsManager : IRatingsManager
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RatingsManager(IRatingRepository ratingRepository, IStoreRepository storeRepository,
        IUserRepository userRepository, IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _storeRepository = storeRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Rating> CreateRating(Rating rating)
        {
            var newRating = await _ratingRepository.CreateAsync(rating);
            await _userRepository.AddToReputation(rating.UserId, 1);
            var storeEntity = await _storeRepository.GetStoreById(rating.Store.Id);

            return new Rating
            {
                Id = newRating.Id,
                Comment = rating.Comment,
                Points = rating.Points,
                UserId = rating.UserId,
                Store = _mapper.Map<Store>(storeEntity),
                CreatedAt = rating.CreatedAt
            };
        }
        
        public async Task<IList<UserRating>> QueryByCoordinatesAsync(decimal latitude, decimal longitude)
        {
            var storeEntities = (await _storeRepository.QueryByCoordinates(latitude, longitude)).ToList();
            var storeIds = storeEntities.Select(s => s.Id);
            var ratingEntities = await _ratingRepository.GetRatingsForStoresAsync(storeIds);
            return await BuildRatingsFromEntities(ratingEntities, storeEntities);
        }

        public async Task<RatingsList> GetRatingsByStoreId(int storeId)
        {
            var storeEntity = await _storeRepository.GetStoreById(storeId);
            var storeEntities = new List<StoreEntity> {storeEntity};
            var ratingEntities = await _ratingRepository.GetRatingsForStoreAsync(storeId);

            return new RatingsList
            {
                Ratings = await BuildRatingsFromEntities(ratingEntities, storeEntities),
                TotalRecords = await _ratingRepository.GetRatingsCountByStoreIdAsync(storeId)
            };
        }

        public async Task<UserRatingsList> GetRecentRatings(int userId)
        {
            var ratingEntities = await _ratingRepository.GetRecentRatingsAsync(userId);
            var storeIds = ratingEntities.Select(r => r.StoreId).ToList();
            var storeEntities = await _storeRepository.GetStoresByIds(storeIds);
            
            return new UserRatingsList
            {
                Ratings = await BuildRatingsFromEntities(ratingEntities, storeEntities),
                TotalRecords = await _ratingRepository.GetRatingsCountByUserIdAsync(userId)
            };
        }

        private async Task<List<UserRating>> BuildRatingsFromEntities(IList<RatingEntity> ratingEntities, IList<StoreEntity> storeEntities)
        {
            var ratingsTasks = ratingEntities.Join(storeEntities, r
                => r.StoreId, s
                => s.Id, async (rating, store) => new UserRating
            {
                Id = rating.Id,
                User = _mapper.Map<User>(await _userRepository.GetById(rating.UserId)),
                Comment = rating.Comment,
                Points = rating.Points,
                Store = _mapper.Map<Store>(store),
                CreatedAt = rating.CreatedAt
            });
            
            return (await Task.WhenAll(ratingsTasks)).ToList();
        }
    }
}