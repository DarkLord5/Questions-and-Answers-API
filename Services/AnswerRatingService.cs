using Microsoft.EntityFrameworkCore;
using Questions_and_Answers_API.Data;
using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Services
{
    public class AnswerRatingService : IAnswerRatingService
    {
        private readonly QAAppContext _context;
        public AnswerRatingService(QAAppContext context)
        {
            _context = context;
        }

        public async Task<int> GetAnswerRating(Guid id)
        {
            int score = 0;

            var ratings = await _context.AnswersRating.Where(qr => qr.AnswerId == id).ToListAsync();

            foreach (var rating in ratings)
            {
                score += rating.Mark;
            }

            return score;
        }

        public async Task CreateRating(User currentUser, Guid answerId, bool mark)
        {
            var answer = await _context.Answers.Where(a => a.Id == answerId).FirstAsync();

            if (!_context.AnswersRating.Any(a => (a.AnswerId == answerId) && (a.UserId == currentUser.Id)))
            {
                int score = mark ? 1 : -1;

                var rating = new AnswerRating() { Mark = score, Answer = answer, User = currentUser };

                _context.AnswersRating.Add(rating);

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteRating(User currentUser, Guid answerId)
        {
            if (_context.AnswersRating.Any(a => (a.AnswerId == answerId) && (a.UserId == currentUser.Id)))
            {
                var rating = await _context.AnswersRating.Where
                (qr => (qr.UserId == currentUser.Id) && (qr.AnswerId == answerId)).FirstAsync();

                _context.AnswersRating.Remove(rating);

                await _context.SaveChangesAsync();
            }
        }
    }
}