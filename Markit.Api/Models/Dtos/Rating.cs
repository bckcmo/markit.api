namespace Markit.Api.Models.Dtos
{
    public class Rating
    {
        public int Id { get; set; }
        public Store Store { get; set; }
        public string Comment { get; set; }
        public int Points { get; set; }
    }
}