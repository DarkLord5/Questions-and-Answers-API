using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Services
{
    public interface ITagService
    {
        public Task<List<Tag>> GetAllTagsAsync();
        public Task<Tag> CreateTagAsync(Tag tag);
        public Task<Tag> UpdateTagAsync(Guid id, Tag tag);
        public Task<List<Tag>> DeleteTagAsync(Guid id);
    }
}