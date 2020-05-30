using System;

namespace Markit.Api.Models.Dtos
{
    public class PriceAttribution
    {
        public string UserName { get; set; }
        public int UserReputation { get; set; }
        public string UserLevel { get; set; }
        public int UserId { get; set; }
        public DateTime SubmittedDate { get; set; }
    }
}