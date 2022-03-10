using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Questions_and_Answers_API.Data;
using Questions_and_Answers_API.Exceptions;
using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Services
{
    public class AnswerService : IAnswerService
    {
        const string Admin_Const = "admin";

        readonly private QAAppContext _context;
        readonly private UserManager<User> _userManager;
        public AnswerService(QAAppContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<List<Answer>> GetAnswersAsync(Guid questionId) =>
            await _context.Answers.Where(a => a.QuestionId == questionId).ToListAsync();


        public async Task<List<Answer>> CreateAnswerAsync(Answer answer, User currentUser)
        {
            var parentQuestion = await _context.Questions.Where(q => q.Id == answer.QuestionId).FirstAsync();

            if (string.IsNullOrEmpty(answer.Text))
            {
                throw new BadRequestException("There is no text in your answer!");
            }

            answer.User = currentUser;
            answer.TimeOfCreation = DateTime.UtcNow;
            answer.TimeOfUpdate = DateTime.UtcNow;
            answer.Tag = await _context.Tags.Where(t => t.Id == parentQuestion.TagId).FirstAsync();

            _context.Entry(answer).State = EntityState.Detached;

            _context.Answers.Add(answer);

            await _context.SaveChangesAsync();

            return await GetAnswersAsync(parentQuestion.Id);
        }


        public async Task<List<Answer>> UpdateAnswerAsync(Answer answer, Guid answerId, Guid questionId)
        {
            var newAnswer = await _context.Answers.Where(a => a.Id == answerId).FirstAsync();

            var user = await _userManager.Users.Where(u => u.Id == answer.UserId).FirstAsync();

            if (string.IsNullOrEmpty(answer.Text) || (newAnswer == null) ||
                ((answer.UserId != newAnswer.UserId) && (!await _userManager.IsInRoleAsync(user, Admin_Const))))
            {
                throw new BadRequestException("You are not the owner of this answer or your new answer is empty!");
            }

            newAnswer.Text = answer.Text;
            newAnswer.TimeOfUpdate = DateTime.UtcNow;

            _context.Entry(newAnswer).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return await GetAnswersAsync(questionId);
        }


        public async Task<List<Answer>> DeleteAnswerAsync(Guid questionId, Answer answer)
        {
            var oldAnswer = await _context.Answers.FindAsync(answer.Id);

            var user = await _userManager.Users.Where(u => u.Id == answer.UserId).FirstAsync();

            if ((oldAnswer == null) ||
                ((answer.UserId != oldAnswer.UserId) && (!await _userManager.IsInRoleAsync(user, Admin_Const))))
                throw new BadRequestException("You are not the owner of this answer!");

            _context.Answers.Remove(answer);

            await _context.SaveChangesAsync();

            return await GetAnswersAsync(questionId);
        }
    }
}
