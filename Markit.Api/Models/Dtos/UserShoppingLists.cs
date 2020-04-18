using System.Collections.Generic;

namespace Markit.Api.Models.Dtos
{
    public class UserShoppingLists
    {
        public int UserId { get; set; }
        public List<ShoppingList> Lists { get; set; }
    }
}