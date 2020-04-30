using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Models.Entities;

namespace Markit.Api.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<TagEntity>> QueryTags(string tag, int limit);
    }
}