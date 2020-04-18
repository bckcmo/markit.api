using System;

namespace Markit.Api.Models.Entities
{
    public class UserPriceEntity
    {
        public int Id { get; set; }
        public int StoreItemsId { get; set; }
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public bool IsSalePrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}