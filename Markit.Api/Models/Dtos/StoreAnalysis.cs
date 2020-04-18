namespace Markit.Api.Models.Dtos
{
    public class StoreAnalysis
    {
        public Store Store { get; set; }
        public decimal TotalPrice { get; set; }
        public int Staleness { get; set; }
        public int PriceRank { get; set; }
        public int PriceAndStalenessRank { get; set; }
    }
}