using System;

namespace Markit.Api.Models.Entities
{
    public class ListTagEntity
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public int TagId { get; set; }
        public int Quantity { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}