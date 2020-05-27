using System;
using System.Collections.Generic;

namespace Markit.Api.Models.Dtos
{
    public class UserPrice
    {
        public int Id { get; set; }
        public User User { get; set; }
        public List<string> TagNames { get; set; }
        public Store Store { get; set; }
        public decimal Price { get; set; }
        public Item Item { get; set; }
        public bool IsSalePrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}