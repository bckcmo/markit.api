namespace Markit.Api.Models.Dtos
{
    public class UserPrice
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public bool IsSalePrice { get; set; }
    }
}