using Microsoft.EntityFrameworkCore;
using Questions_and_Answers_API.Data;
using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Services
{
    public class AnswerRatingService: IAnswerRatingService
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

        public async Task CreateRating(User currentUser, Answer answer, bool mark)
        {
            int score = mark ? 1 : 0;

            var rating = new AnswerRating() { Mark = score, Answer = answer, User = currentUser };

            _context.AnswersRating.Add(rating);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRating(User currentUser, Answer answer)
        {
            var rating = await _context.AnswersRating.Where(qr => (qr.User == currentUser) && (qr.Answer == answer)).FirstAsync();
            if (rating != null)
            {
                _context.AnswersRating.Remove(rating);

                await _context.SaveChangesAsync();
            }
        }
    }
}