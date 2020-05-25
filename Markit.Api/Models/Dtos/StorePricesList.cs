using System.Collections.Generic;

namespace Markit.Api.Models.Dtos
{
    public class StorePricesList
    {
        public List<UserPrice> UserPrices { get; set; }
        public int TotalRecords { get; set; }
    }
}