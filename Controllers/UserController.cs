using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questions_and_Answers_API.Models;
using Questions_and_Answers_API.Services;
using Questions_and_Answers_API.ViewModel;

namespace Questions_and_Answers_API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        
        [HttpPost]
        [Route("api/Registration")]
        public async Task<ActionResult<User>> Registration(RegistrationViewModel newUser) => Ok(await _userService.Registration(newUser));

        [HttpPost]
        [Route("api/Login")]
        public async Task<ActionResult<bool>> Login(LoginViewModel user) => Ok(await _userService.Login(user));

        [HttpPost]
        [Route("api/Logout")]
        [Authorize(Roles = "admin,user")]
        public async Task Logout() => await _userService.Logout();
    }
}