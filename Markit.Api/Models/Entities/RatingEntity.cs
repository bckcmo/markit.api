using System;

namespace Markit.Api.Models.Entities
{
    public class RatingEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StoreId { get; set; }
        public string Comment { get; set; }
        public int Points { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}