using System;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Models.Entities
{
    public class StoreEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int PostalCode { get; set; }
        public Coordinate Coordinate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}