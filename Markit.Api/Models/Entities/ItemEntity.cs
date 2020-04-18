using System;

namespace Markit.Api.Models.Entities
{
    public class ItemEntity
    {
        public int Id { get; set; }
        public string Upc { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}