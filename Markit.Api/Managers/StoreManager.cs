using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Managers
{
    public class StoreManager : IStoreManager
    {
        private readonly IStoreRepository _storeRepository;

        public StoreManager(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }
        
        public async Task<Store> CreateStoreAsync(Store store)
        {
            var storeEntity = await _storeRepository.CreateStore(store);
            
            return new Store
            {
                Id = storeEntity.Id,
                Name = storeEntity.Name,
                StreetAddress = storeEntity.StreetAddress,
                City = storeEntity.City,
                State = storeEntity.State,
                PostalCode = storeEntity.PostalCode,
                Coordinate = new Coordinate
                {
                    Latitude = storeEntity.Latitude,
                    Longitude = storeEntity.Longitude
                },
                GoogleId = storeEntity.GoogleId,
                AverageRating = storeEntity.AverageRating
            };
        }

        public async Task<IEnumerable<Store>> QueryByCoordinatesAsync(decimal lat, decimal lon)
        {
            var storeEntities = await _storeRepository.QueryByCoordinates(lat, lon);
            return storeEntities.Select(store => new Store
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
                },
                GoogleId = store.GoogleId,
                AverageRating = store.AverageRating
            });
        }

        public async Task<Store> GetById(int id)
        {
            var storeEntity = await _storeRepository.GetStoreById(id);

            return new Store
            {
                Id = storeEntity.Id,
                Name = storeEntity.Name,
                StreetAddress = storeEntity.StreetAddress,
                City = storeEntity.City,
                State = storeEntity.State,
                PostalCode = storeEntity.PostalCode,
                Coordinate = new Coordinate
                {
                    Latitude = storeEntity.Latitude,
                    Longitude = storeEntity.Longitude
                },
                GoogleId = storeEntity.GoogleId,
                AverageRating = storeEntity.AverageRating
            };
        }

        public async Task<Store> PutStore(Store store)
        {
            var storeEntity = await _storeRepository.ReplaceStore(store);

            return new Store
            {
                Id = storeEntity.Id,
                Name = storeEntity.Name,
                StreetAddress = storeEntity.StreetAddress,
                City = storeEntity.City,
                State = storeEntity.State,
                PostalCode = storeEntity.PostalCode,
                Coordinate = new Coordinate
                {
                    Latitude = storeEntity.Latitude,
                    Longitude = storeEntity.Longitude
                },
                GoogleId = storeEntity.GoogleId,
                AverageRating = storeEntity.AverageRating
            };
        }

        public async Task Delete(int id)
        {
            await _storeRepository.DeleteStoreById(id);
        }
    }
}