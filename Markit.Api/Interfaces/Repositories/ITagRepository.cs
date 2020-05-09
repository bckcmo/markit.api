using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Models.Entities;

namespace Markit.Api.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<TagEntity>> QueryTags(string name, string upc, int limit);
        Task<IEnumerable<ListTagEntity>> GetListTags(int listId);
        Task<TagEntity> GetTagById(int id);
        Task<List<TagEntity>> GetTagsByItemId(int itemId);
        Task<List<TagEntity>> GetTagsByItemUpc(string upc);
    }
}