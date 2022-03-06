using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Questions_and_Answers_API.Data;
using Questions_and_Answers_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Questions_and_Answers_API.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly QAAppContext _context;
        public AnswerService(QAAppContext context)
        {
            _context = context;
        }

        public async Task<List<Answer>> GetAnswersAsync(Guid questionId) => 
            await _context.Answers.Where(a=>a.QuestionId==questionId).ToListAsync();
        

        public async Task<List<Answer>> CreateAnswerAsync(Answer answer, Question question, User currentUser)
        {
            if (answer.Text == null)
            {
                return await GetAnswersAsync(question.Id);
            }

            answer.Question = question;

            answer.User= currentUser;

            answer.TimeOfCreation = DateTime.Now;

            answer.TimeOfUpdate = DateTime.Now;

            answer.Tag = answer.Question.Tag;

            _context.Answers.Add(answer);

            await _context.SaveChangesAsync();

            return await GetAnswersAsync(question.Id);
        }


        public async Task<List<Answer>> UpdateAnswerAsync(Answer answer, Guid answerId, Guid questionId)
        {
            if((answer.Text == null)||(answerId!=answer.Id)||(!_context.Answers.Any(a=>a.Id==answerId)))
            {
                return await GetAnswersAsync(questionId);
            }
            answer.TimeOfUpdate = DateTime.Now;

            _context.Entry(answer).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return await GetAnswersAsync(questionId);
        }


        public async Task<List<Answer>> DeleteAnswerAsync(Guid questionId, Guid answerId)
        {
            var answer = await _context.Answers.FindAsync(answerId);

            if (answer == null)  
                return await GetAnswersAsync(questionId); 

            _context.Answers.Remove(answer);

            await _context.SaveChangesAsync();

            return await GetAnswersAsync(questionId);
        }
    }
}
