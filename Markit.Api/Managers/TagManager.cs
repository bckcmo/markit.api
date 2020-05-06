using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Managers
{
    public class TagManager : ITagManager
    {
        private readonly ITagRepository _tagRepository;

        public TagManager(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IList<Tag>> QueryTagsAsync(string tag, string upc, int limit)
        {
            var tags = await _tagRepository.QueryTags(tag, upc, limit);
            
            return tags.Select(t => new Tag
            {
                Id = t.Id,
                Name = t.Name,
            }).ToList();
        }
    }
}