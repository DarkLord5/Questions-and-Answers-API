using Microsoft.EntityFrameworkCore;
using Questions_and_Answers_API.Data;
using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Services
{
    public class QuestionRatingService : IQuestionRatingService
    {

        private readonly QAAppContext _context;
        public QuestionRatingService(QAAppContext context)
        {
            _context = context;
        }

        public async Task<int> GetQuestionRating(Guid id)
        {
            int score = 0;

            var ratings = await _context.QuestionsRating.Where(qr => qr.QuestionId == id).ToListAsync();

            foreach(var rating in ratings)
            {
                score += rating.Mark;
            }

            return score;
        }

        public async Task CreateRating(User currentUser, Question question, bool mark)
        {
            int score = mark ? 1 : 0;

            var rating = new QuestionRating() { Mark = score, Question = question, User = currentUser };

            _context.QuestionsRating.Add(rating);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRating(User currentUser, Question question)
        {
            var rating = await _context.QuestionsRating.Where(qr => (qr.User == currentUser) && (qr.Question == question)).FirstAsync();
            if(rating != null)
            {
                _context.QuestionsRating.Remove(rating);

                await _context.SaveChangesAsync();
            }
        }
    }
}
