using System;
using System.Collections.Generic;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Models.Entities
{
    public class ShoppingListEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}