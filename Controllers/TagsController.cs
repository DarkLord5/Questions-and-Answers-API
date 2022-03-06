#nullable disable
using Microsoft.AspNetCore.Mvc;
using Questions_and_Answers_API.Models;
using Questions_and_Answers_API.Services;

namespace Questions_and_Answers_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET: api/Tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags() => Ok(await _tagService.GetAllTagsAsync());
        

        // PUT: api/Tags/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateTag(Guid id, Tag tag) => Ok(await _tagService.UpdateTagAsync(id, tag));
        

        // POST: api/Tags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tag>> CreateTag(Tag tag) => Ok(await _tagService.CreateTagAsync(tag));
        

        // DELETE: api/Tags/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTag(Guid id) => Ok(await _tagService.DeleteTagAsync(id));
        
    }
}
