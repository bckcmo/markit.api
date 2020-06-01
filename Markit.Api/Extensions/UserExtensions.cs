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
            {UserLevels.LevelOne, UserLevelNames.LevelOne},
            {UserLevels.LevelTwo, UserLevelNames.LevelTwo},
            {UserLevels.LevelThree, UserLevelNames.LevelThree},
            {UserLevels.LevelFour, UserLevelNames.LevelFour},
            {UserLevels.LevelFive, UserLevelNames.LevelFive}
        }; 
        
        public static string GetUserLevel(this User user)
        {
            return userLevelDictionary.FirstOrDefault(i => user.Reputation < (int) i.Key).Value
                ?? UserLevelNames.LevelSix;
        }
    }
}