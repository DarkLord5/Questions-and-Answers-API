using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Services
{
    public interface IAnswerRatingService
    {
        public Task<int> GetAnswerRating(Guid id);
        public Task CreateRating(User currentUser, Guid answerId, bool mark);
        public Task DeleteRating(User currentUser, Guid answerId);
    }
}
