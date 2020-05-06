using System.Collections.Generic;
using Markit.Api.Models.Entities;

namespace Markit.Api.Comparers
{
    class TagEntityCompare : IEqualityComparer<TagEntity>
    {
        public bool Equals(TagEntity x, TagEntity y)
        {
            return x.Id == y.Id;
        }
        
        public int GetHashCode(TagEntity codeh)
        {
            return codeh.Id.GetHashCode();
        }
    }
}