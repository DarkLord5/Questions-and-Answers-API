namespace Questions_and_Answers_API.Models
{
    public class Question
    {
        public Guid Id { get; set;}
        public string? Title { get; set;}
        public string? Description { get; set;}
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfUpdate { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }

        public Guid TagId { get; set; }
        public Tag? Tag { get; set; }

    }
}
