using Questions_and_Answers_API.Models;
using Questions_and_Answers_API.ViewModel;

namespace Questions_and_Answers_API.Services
{
    public interface IUserService
    {
        public Task<User> Registration(RegistrationViewModel newUser);
        public Task<bool> Login(LoginViewModel user);
        public Task Logout();

    }
}
