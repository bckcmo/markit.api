using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Interfaces.Managers
{
    public interface ITagManager
    {
        Task<IList<Tag>> QueryTagsAsync(string tag, int limit);
    }
}