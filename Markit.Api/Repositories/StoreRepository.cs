using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Interfaces.Utils;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;
using MySql.Data.MySqlClient;

namespace Markit.Api.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly string _connectionString;
        
        private IDbConnection connection => new MySqlConnection(_connectionString);

        public StoreRepository(IDatabaseUtil databaseUtil)
        {
            _connectionString = databaseUtil.GetConnectionString();
        }

        public async Task<StoreEntity> CreateStore(Store store)
        {
            using var conn = connection;
            var insertQuery =
                @"INSERT into stores (Name, StreetAddress, City, State, PostalCode, Latitude, Longitude, GoogleId) 
                    Values (@Name, @StreetAddress, @City, @State, @Postalcode, @Latitude, @Longitude, @GoogleId)
                    ON DUPLICATE KEY UPDATE Id = LAST_INSERT_ID(id), Name = @Name;
                    SELECT * from stores WHERE id = LAST_INSERT_ID()";
            
            conn.Open();
            
            var result = await conn.QueryAsync<StoreEntity>(insertQuery, new {
                Name = store.Name,
                StreetAddress = store.StreetAddress,
                City = store.City,
                State = store.State,
                PostalCode = store.PostalCode,
                Latitude = store.Coordinate.Latitude,
                Longitude = store.Coordinate.Longitude,
                GoogleId = store.GoogleId
            });
            return result.Single();
        }

        public async Task<IList<StoreEntity>> QueryByCoordinates(decimal lat, decimal lon, int limit)
        {
            var limitString = limit == 0 ? ";" : "LIMIT 0 , @limit;";
            
            using var conn = connection;
            var query =
                @"SELECT *, 
                    (3956 * 
                    acos (cos ( radians(@lat) ) * cos( radians( Latitude ) ) * 
                    cos( radians( @lon ) - radians(Longitude) ) + 
                    sin ( radians(@lat) ) * sin( radians( Latitude ) ))) 
                    AS distance FROM stores HAVING distance < 10 ORDER BY distance " + limitString;
            
            conn.Open();

            return (await conn.QueryAsync<StoreEntity>(query, new {lat, lon, limit})).ToList();
        }

        public async Task<IList<StoreEntity>> QueryByCoordinates(decimal lat, decimal lon)
        {
            return await this.QueryByCoordinates(lat, lon, 20);
        }

        public async Task<StoreEntity> GetStoreById(int id)
        {
            using var conn = connection;
            
            conn.Open();
            
            var query = @"SELECT * FROM stores WHERE Id = @id";

            var store = await conn.QueryAsync<StoreEntity>(query, new {Id = id});
            return store.Single();
        }

        public async Task<IList<StoreItemEntity>> GetStoreItemsFromStoreIds(List<int> storeIds)
        {
            using var conn = connection;
            
            conn.Open();
            
            var query = @"SELECT * FROM storeitems WHERE storeId IN @storeIds ORDER BY CreatedDate DESC LIMIT 20";
            
            var results = await conn.QueryAsync<StoreItemEntity>(query, new { storeIds });

            return results.ToList();
        }

        public async Task<StoreEntity> ReplaceStore(Store store)
        {
            using var conn = connection;
            var insertQuery =
                @"UPDATE stores SET Name = @Name, StreetAddress = @StreetAddress, City = @City, State = @State, 
                PostalCode = @Postalcode, Latitude = @Latitude, Longitude = @Longitude, GoogleId = @GoogleId
                WHERE Id = @Id;
                SELECT * from stores WHERE id = @Id";
            
            conn.Open();
            
            var result = await conn.QueryAsync<StoreEntity>(insertQuery, new {
                Id = store.Id,
                Name = store.Name,
                StreetAddress = store.StreetAddress,
                City = store.City,
                State = store.State,
                PostalCode = store.PostalCode,
                Latitude = store.Coordinate.Latitude,
                Longitude = store.Coordinate.Longitude
            });
            return result.Single();
        }

        public async Task DeleteStoreById(int id)
        {
            using var conn = connection;
            
            conn.Open();
            
            var query = @"DELETE FROM stores WHERE Id = @id";
            
            await conn.ExecuteAsync(query, new { id });
        }

        public async Task<IList<StoreEntity>> GetStoresByIds(List<int> ids)
        {
            using var conn = connection;
            
            conn.Open();
            
            var query = @"SELECT * FROM stores WHERE Id IN @ids;";
            
            var results = await conn.QueryAsync<StoreEntity>(query, new { ids });

            return results.ToList();
        }
    }
}