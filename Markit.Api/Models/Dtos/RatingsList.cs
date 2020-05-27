using System.Collections.Generic;

namespace Markit.Api.Models.Dtos
{
    public class RatingsList
    {
        public List<UserRating> Ratings { get; set; }
        public int TotalRecords { get; set; }
    }
}