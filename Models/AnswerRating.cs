namespace Questions_and_Answers_API.Models
{
    public class AnswerRating
    {
        public string? UserId { get; set; }
        public User? User { get; set; }

        public Guid AnswerId { get; set; }
        public Answer? Answer { get; set; }

        public int Mark { get; set; }
    }
}
