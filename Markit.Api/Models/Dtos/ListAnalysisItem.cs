using System.Collections.Generic;

namespace Markit.Api.Models.Dtos
{
    public class ListAnalysisItem
    {
        public List<string> TagNames { get; set; }
        public decimal Price { get; set; }
        public Item Item { get; set; }
        public bool IsSalePrice { get; set; }
    }
}