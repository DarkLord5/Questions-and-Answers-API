using Microsoft.AspNetCore.Identity;
using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<User> Registration(string email, string password, string name, string surname)
        {
            
            User user = new() { Email = email, UserName = email, FirstName = name, SecondName = surname};
            
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                
                await _signInManager.SignInAsync(user, false);

                return user;
            }

            return new User();
        }

        public async Task<bool> Login(string email, string password)
        {
            var result =
                    await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (result.Succeeded)
            {

                return true;
            }

            return false;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
