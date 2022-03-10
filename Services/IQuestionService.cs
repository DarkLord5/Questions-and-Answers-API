using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Services
{
    public interface IQuestionService
    {
        public Task<List<Question>> GetAlQuestionsAsync();
        public Task<Question> GetQuestionAsync(Guid questionId);
        public Task<Question> CreateQuestionAsync(Question question, User currentUser);
        public Task<List<Question>> FindByTagNameAsync(string tagName);
        public Task<List<Question>> DeleteQuestion(Question question);
        public Task<Question> UpdateQuestion(Question question, Guid id);
    }
}