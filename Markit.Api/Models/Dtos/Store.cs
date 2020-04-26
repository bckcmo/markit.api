namespace Markit.Api.Models.Dtos
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}