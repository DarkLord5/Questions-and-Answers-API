using Microsoft.AspNetCore.Identity;

namespace Questions_and_Answers_API.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
    }
}
