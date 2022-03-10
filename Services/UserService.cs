﻿using Microsoft.AspNetCore.Identity;
using Questions_and_Answers_API.Exceptions;
using Questions_and_Answers_API.Models;
using Questions_and_Answers_API.ViewModel;

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

        public async Task<User> Registration(RegistrationViewModel newUser)
        {

            User user = new()
            {
                Email = newUser.Email,
                UserName = newUser.Email,
                FirstName = newUser.Name,
                SecondName = newUser.Surname
            };

            var result = await _userManager.CreateAsync(user, newUser.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");

                await _signInManager.SignInAsync(user, false);

                return user;
            }

            throw new BadRequestException("Registration failed!");
        }

        public async Task<bool> Login(LoginViewModel user)
        {

            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);

            if (result.Succeeded)
            {
                return true;
            }

            throw new BadRequestException("Authorization failed!");
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
