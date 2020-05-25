using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Managers
{
    public class StoreManager : IStoreManager
    {
        private readonly IStoreRepository _storeRepository;
        private readonly Mapper _mapper;

        public StoreManager(IStoreRepository storeRepository, IMapper mapper)
        {
            _storeRepository = storeRepository;
        }
        
        public async Task<Store> CreateStoreAsync(Store store)
        {
            var storeEntity = await _storeRepository.CreateStore(store);
            return _mapper.Map<Store>(storeEntity);
        }

        public async Task<IEnumerable<Store>> QueryByCoordinatesAsync(decimal lat, decimal lon)
        {
            var storeEntities = await _storeRepository.QueryByCoordinates(lat, lon);
            return storeEntities.Select(_mapper.Map<Store>);
        }

        public async Task<Store> GetById(int id)
        {
            var storeEntity = await _storeRepository.GetStoreById(id);
            return _mapper.Map<Store>(storeEntity);
        }

        public async Task<Store> PutStore(Store store)
        {
            var storeEntity = await _storeRepository.ReplaceStore(store);
            return _mapper.Map<Store>(storeEntity);
        }

        public async Task Delete(int id)
        {
            await _storeRepository.DeleteStoreById(id);
        }
    }
}