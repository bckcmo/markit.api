using System.Collections.Generic;

namespace Markit.Api.Models.Dtos
{
    public class UserRatingsList
    {
        public List<Rating> Ratings { get; set; }
        public int TotalRecords { get; set; }
    }
}