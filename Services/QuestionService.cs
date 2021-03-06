using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Questions_and_Answers_API.Data;
using Questions_and_Answers_API.Exceptions;
using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Services
{
    public class QuestionService : IQuestionService
    {
        const string Admin_Const = "admin";

        readonly private QAAppContext _context;
        readonly private UserManager<User> _userManager;
        public QuestionService(QAAppContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<List<Question>> GetAlQuestionsAsync() => await _context.Questions.ToListAsync();

        public async Task<Question> GetQuestionAsync(Guid questionId) =>
            await _context.Questions.Where(q => q.Id == questionId).FirstAsync();


        public async Task<Question> CreateQuestionAsync(Question question, User currentUser)
        {
            if (string.IsNullOrEmpty(question.Description) || string.IsNullOrEmpty(question.Title))
            {
                throw new BadRequestException("Your question doesn't contain a title or description!");
            }

            var tag = await _context.Tags.Where(t => t.Id == question.TagId).FirstAsync();

            question.User = currentUser;
            question.Tag = tag;
            question.DateOfCreation = DateTime.UtcNow;
            question.DateOfUpdate = DateTime.UtcNow;

            _context.Entry(question).State = EntityState.Detached;

            _context.Questions.Add(question);

            await _context.SaveChangesAsync();

            return question;
        }


        public async Task<List<Question>> FindByTagNameAsync(string tagName)
        {
            if (_context.Tags.Any(t => t.Name == tagName))
            {
                var tag = await _context.Tags.Where(t => t.Name == tagName).FirstAsync();

                return await _context.Questions.Where(q => tag.Id == q.TagId).ToListAsync();
            }

            return new List<Question>();
        }


        public async Task<Question> UpdateQuestion(Question question, Guid id)
        {
            var newQuestion = await _context.Questions.Where(q => q.Id == id).FirstAsync();

            var user = await _userManager.Users.Where(u => u.Id == question.UserId).FirstAsync();

            if (string.IsNullOrEmpty(question.Title) || string.IsNullOrEmpty(question.Description) || (newQuestion == null) ||
                ((question.UserId != newQuestion.UserId) && (!await _userManager.IsInRoleAsync(user, Admin_Const))))
            {
                throw new BadRequestException("Your question doesn't contain a title or description " +
                    "or you are not the owner of this question!");
            }

            newQuestion.Title = question.Title;
            newQuestion.Description = question.Description;
            newQuestion.DateOfUpdate = DateTime.UtcNow;

            _context.Entry(newQuestion).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return newQuestion;
        }

        public async Task<List<Question>> DeleteQuestion(Question question)
        {
            var oldQuesiton = await _context.Questions.FindAsync(question.Id);

            var user = await _userManager.Users.Where(u => u.Id == question.UserId).FirstAsync();

            if ((oldQuesiton == null) ||
                ((question.UserId != oldQuesiton.UserId) && (!await _userManager.IsInRoleAsync(user, Admin_Const))))
                throw new BadRequestException("You are not the owner of this question!");

            _context.Questions.Remove(oldQuesiton);

            await _context.SaveChangesAsync();

            return await GetAlQuestionsAsync();
        }
    }
}