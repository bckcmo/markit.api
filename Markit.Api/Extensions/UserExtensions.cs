using System.Collections.Generic;
using System.Linq;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Statics;

namespace Markit.Api.Extensions
{
    public static class UserExtensions
    {
        private static readonly Dictionary<UserLevels, string> userLevelDictionary = new Dictionary<UserLevels, string>
        {
            {UserLevels.Shopper, UserLevelNames.Shopper},
            {UserLevels.SuperShopper, UserLevelNames.SuperShopper},
            {UserLevels.UltraSuperShopper, UserLevelNames.UltraSuperShopper},
            {UserLevels.ExtremeUltraSuperShopper, UserLevelNames.ExtremeUltraSuperShopper},
            {UserLevels.TheMostExtremeUltraSuperShopper, UserLevelNames.TheMostExtremeUltraSuperShopper}
        }; 
        
        public static string GetUserLevel(this User user)
        {
            return userLevelDictionary.FirstOrDefault(i => user.Reputation < (int) i.Key).Value
                ?? UserLevelNames.TheVeryMostExtremeUltraSuperShopper;
        }
    }
}