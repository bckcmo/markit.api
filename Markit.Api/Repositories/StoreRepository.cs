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
                @"INSERT into stores (Name, StreetAddress, City, State, PostalCode, Latitude, Longitude) 
                    Values (@Name, @StreetAddress, @City, @State, @Postalcode, @Latitude, @Longitude);
                    SELECT * from stores WHERE id = LAST_INSERT_ID()";
            
            conn.Open();
            
            var result = await conn.QueryAsync<StoreEntity>(insertQuery, new {
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

        public async Task<IEnumerable<StoreEntity>> QueryByCoordinates(decimal lat, decimal lon)
        {
            using var conn = connection;
            var query =
                @"SELECT Id, Name, StreetAddress, City, State, PostalCode, Latitude, Longitude, (3956 * 
                    acos (cos ( radians(@lat) ) * cos( radians( Latitude ) ) * 
                    cos( radians( @lon ) - radians(Longitude) ) + 
                    sin ( radians(@lat) ) * sin( radians( Latitude ) ))) 
                    AS distance FROM stores HAVING distance < 10 ORDER BY distance 
                    LIMIT 0 , 20;";
            
            conn.Open();

            return await conn.QueryAsync<StoreEntity>(query, new {lat, lon});
        }
    }
}