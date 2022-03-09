using Microsoft.EntityFrameworkCore;
using Questions_and_Answers_API.Data;
using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Services
{
    public class TagService : ITagService
    {
        private readonly QAAppContext _context;

        public TagService(QAAppContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAllTagsAsync() => await _context.Tags.ToListAsync();


        public async Task<Tag> CreateTagAsync(Tag tag)
        {
            var check = _context.Tags.Where(t => t.Name == tag.Name).First();

            if (!(_context.Tags.Any(t => t.Name == tag.Name) && string.IsNullOrEmpty(tag.Name)))
            {
                _context.Tags.Add(tag);

                await _context.SaveChangesAsync();
            }

            return tag;
        }

        public async Task<List<Tag>> DeleteTagAsync(Guid id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null)
            {
                return await GetAllTagsAsync();
            }

            _context.Tags.Remove(tag);

            await _context.SaveChangesAsync();

            return await GetAllTagsAsync();
        }


        public async Task<Tag> UpdateTagAsync(Guid id, Tag tag)
        {
            var newTag = new Tag() { Id = id, Name = tag.Name };

            if (string.IsNullOrEmpty(newTag.Name))
            {
                return await _context.Tags.Where(t => t.Id == id).FirstAsync();
            }

            if (!_context.Tags.Any(e => e.Id == id)) { return new Tag(); }

            _context.Entry(newTag).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return newTag;
        }


    }
}
