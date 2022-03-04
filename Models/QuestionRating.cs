namespace Questions_and_Answers_API.Models
{
    public class QuestionRating
    {
        //public string UserId { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        
        public Guid QuestionId { get; set; }
        public Question? Question { get; set; }

        public int Mark { get; set; }
    }
}
