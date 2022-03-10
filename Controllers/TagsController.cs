#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questions_and_Answers_API.Models;
using Questions_and_Answers_API.Services;

namespace Questions_and_Answers_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags() => Ok(await _tagService.GetAllTagsAsync());


        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateTag(Guid id, Tag tag) => Ok(await _tagService.UpdateTagAsync(id, tag));


        [HttpPost]
        public async Task<ActionResult<Tag>> CreateTag(Tag tag) => Ok(await _tagService.CreateTagAsync(tag));


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTag(Guid id) => Ok(await _tagService.DeleteTagAsync(id));

    }
}
