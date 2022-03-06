using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Services
{
    public interface IAnswerService
    {
        public Task<List<Answer>> GetAnswersAsync(Guid questionId);
        public Task<List<Answer>> CreateAnswerAsync(Answer answer, Question question, User currentUser);
        public Task<List<Answer>> UpdateAnswerAsync(Answer answer, Guid answerId, Guid questionId);
        public Task<List<Answer>> DeleteAnswerAsync(Guid questionId, Guid answerId);
    }
}