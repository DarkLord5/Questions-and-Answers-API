using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Services
{
    public interface IQuestionService
    {
        public Task<List<Question>> GetAlQuestionsAsync();
        public Task<Question> GetQuestionAsync(Guid questionId);
        public Task<Question> CreateQuestionAsync(Question question, User currentUser, Tag tag);
        public Task<List<Question>> FindByTagNameAsync(string tagName);
        public Task<List<Question>> DeleteQuestion(Guid questionId);
        public Task<Question> UpdateQuestion(Question question, Guid id);
    }
}