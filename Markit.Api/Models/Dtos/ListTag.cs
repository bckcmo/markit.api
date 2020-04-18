namespace Markit.Api.Models.Dtos
{
    public class ListTag
    {
        public int Id { get; set; }
        public Tag Tag { get; set; }
        public int Quantity { get; set; }
        public string Comment { get; set; }
    }
}