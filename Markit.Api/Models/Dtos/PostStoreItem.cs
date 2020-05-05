using System.Collections.Generic;

namespace Markit.Api.Models.Dtos
{
    public class PostStoreItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StoreId { get; set; }
        public string Upc { get; set; }
        public decimal Price { get; set; }
        public bool IsSalePrice { get; set; }
        public List<string> Tags { get; set; }
    }
}