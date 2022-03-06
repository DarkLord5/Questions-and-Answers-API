using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Services
{
    public interface IQuestionRatingService
    {
        public Task<int> GetQuestionRating(Guid id);
        public Task CreateRating(User currentUser, Question question, bool mark);
        public Task DeleteRating(User currentUser, Question question);
    }
}
