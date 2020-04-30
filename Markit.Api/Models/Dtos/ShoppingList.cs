using System.Collections.Generic;

namespace Markit.Api.Models.Dtos
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ListTag> ListTags { get; set; }
    }
}