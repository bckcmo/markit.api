using System.Collections.Generic;

namespace Markit.Api.Models.Dtos
{
    public class UserRatingsList
    {
        public List<UserRating> Ratings { get; set; }
        public int TotalRecords { get; set; }
    }
}