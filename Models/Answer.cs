namespace Questions_and_Answers_API.Models
{
    public class Answer
    {
        public Guid Id { get; set; }
        public string? Text { get; set; }
        public DateTime TimeOfCreation { get; set; }
        public DateTime TimeOfUpdate { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }

        public Guid TagId { get; set; }
        public Tag? Tag { get; set; }

        public Guid QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
