using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Services
{
    public interface IUserService
    {
        public Task<User> Registration(string email, string password, string name, string surname);
        public Task<bool> Login(string email, string password);
        public Task Logout();

    }
}
