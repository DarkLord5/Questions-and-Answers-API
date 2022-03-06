using Microsoft.EntityFrameworkCore;
using Questions_and_Answers_API.Data;
using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Services
{
    public class QuestionService : IQuestionService
    {

        private readonly QAAppContext _context;
        public QuestionService(QAAppContext context)
        {
            _context = context;
        }


        public async Task<List<Question>> GetAlQuestionsAsync() => await _context.Questions.ToListAsync();

        public async Task<Question> GetQuestionAsync(Guid questionId)=> await _context.Questions.Where(q=>q.Id==questionId).FirstAsync();


        public async Task<Question> CreateQuestionAsync(Question question, User currentUser, Tag tag)
        {

            if ((question.Description == null) || (question.Title == null)||(!_context.Tags.Any(t=>t.Id == tag.Id)))
            {
                return new Question();
            }

            question.User = currentUser;

            question.Tag = tag;

            question.DateOfCreation = DateTime.Now;

            question.DateOfUpdate = DateTime.Now;

            _context.Questions.Add(question);

            await _context.SaveChangesAsync();

            return question;
        }


        public async Task<List<Question>> FindByTagNameAsync(string tagName) => 
            await _context.Questions.Where(q => q.Tag.Name == tagName).ToListAsync();


        public async Task<Question> UpdateQuestion(Question question, Guid id)
        {

            if ((question.Title == null) || (question.Description==null) || (question.Id != id) || 
                (!_context.Questions.Any(q => q.Id == id)))
            {
                return new Question();
            }

            question.DateOfUpdate = DateTime.Now;

            _context.Entry(question).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return question;

        }

        public async Task<List<Question>> DeleteQuestion(Guid questionId)
        {
            var queston = await _context.Questions.FindAsync(questionId);

            if(queston == null) 
                return await GetAlQuestionsAsync();

            _context.Questions.Remove(queston);

            await _context.SaveChangesAsync();

            return await GetAlQuestionsAsync();
        }
    }
}