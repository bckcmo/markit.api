using System;
using System.Collections.Generic;

namespace Markit.Api.Models.Entities
{
    public class StoreItemEntity
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int StoreId { get; set; }
        public string Upc { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}