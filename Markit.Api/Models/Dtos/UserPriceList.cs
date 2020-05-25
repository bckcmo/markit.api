using System.Collections.Generic;

namespace Markit.Api.Models.Dtos
{
    public class UserPriceList
    {
        public List<UserPrice> UserPrices { get; set; }
        public int TotalRecords { get; set; }
    }
}