#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Questions_and_Answers_API.Data;
using Questions_and_Answers_API.Models;
using Questions_and_Answers_API.Services;
using Questions_and_Answers_API.ViewModel;

namespace Questions_and_Answers_API.Controllers
{

    [ApiController]
    public class QuestionsController : ControllerBase
    {
        readonly private IQuestionService _questionService;
        readonly private IAnswerService _answerService;
        readonly private IQuestionRatingService _questionRatingService;
        readonly private IAnswerRatingService _answerRatingService;
        readonly private UserManager<User> _userManager;

        public QuestionsController(IQuestionService questionService, IAnswerService answerService,
            IQuestionRatingService questionRatingService, IAnswerRatingService answerRatingService, UserManager<User> userManager)
        {
            _questionService = questionService;
            _answerService = answerService;
            _questionRatingService = questionRatingService;
            _answerRatingService = answerRatingService;
            _userManager = userManager;
        }


        [HttpGet]
        [Route("api/Question")]
        public async Task<ActionResult<List<QuestionViewModel>>> GetAllQuestions()
        {
            var questionsWithRatings = new List<QuestionViewModel>();

            var questions = await _questionService.GetAlQuestionsAsync();

            foreach (Question question in questions)
            {
                questionsWithRatings.Add(new QuestionViewModel()
                {
                    Question = question,
                    Rating = await _questionRatingService.GetQuestionRating(question.Id)
                });
            }

            return Ok(questionsWithRatings);
        }

        [HttpGet]
        [Route("api/Question/Filter/{tag}")]
        public async Task<ActionResult<List<QuestionViewModel>>> GetFiltredQuestions(string tag)
        {
            var questionsWithRatings = new List<QuestionViewModel>();

            var questions = await _questionService.FindByTagNameAsync(tag);

            foreach (Question question in questions)
            {
                questionsWithRatings.Add(new QuestionViewModel()
                {
                    Question = question,
                    Rating = await _questionRatingService.GetQuestionRating(question.Id)
                });
            }

            return Ok(questionsWithRatings);
        }


        [HttpGet]
        [Route("api/Question/{id}")]
        public async Task<ActionResult<QuestionWithAnswersViewModel>> GetSelectedQuestion(Guid id)
        {
            var qViewModel = new QuestionViewModel()
            {
                Question = await _questionService.GetQuestionAsync(id),
                Rating = await _questionRatingService.GetQuestionRating(id)
            };

            var answerList = await _answerService.GetAnswersAsync(id);

            var answersWithRating = new List<AnswerViewModel>();

            foreach (var answer in answerList)
            {
                answersWithRating.Add(new AnswerViewModel()
                {
                    Answer = answer,
                    Rating = await _answerRatingService.GetAnswerRating(answer.Id)
                });
            }

            var qaList = new QuestionWithAnswersViewModel
            {
                QuestionViewModel = qViewModel,

                Answers = answersWithRating
            };

            return Ok(qaList);
        }


        [HttpPost]
        [Route("api/Question")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult<Question>> CreateQuestion(Question question)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            return Ok(await _questionService.CreateQuestionAsync(question, currentUser));
        }

        [HttpPost]
        [Route("api/Question/{id}/Answer")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult<List<Answer>>> CreateAnswer(Guid id, Answer answer)
        {
            answer.QuestionId = id;

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            return Ok(await _answerService.CreateAnswerAsync(answer, currentUser));
        }

        [HttpPut]
        [Route("api/Question/{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult<Question>> UpdateQuestion(Guid id, Question question)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            question.UserId = user.Id;

            return Ok(await _questionService.UpdateQuestion(question, id));
        }

        [HttpPut]
        [Route("api/Question/{id}/Answer/{answerId}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult<List<Answer>>> UpdateAnswer(Answer answer, Guid id, Guid answerId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            answer.UserId = user.Id;

            return Ok(await _answerService.UpdateAnswerAsync(answer, answerId, id));
        }

        [HttpDelete]
        [Route("api/Question/{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult<List<Question>>> DeleteQuestion(Guid id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var question = new Question()
            {
                Id = id,
                UserId = user.Id
            };

            return (Ok(await _questionService.DeleteQuestion(question)));
        }



        [HttpDelete]
        [Route("api/Question/{id}/Answer/{answerId}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult<List<Answer>>> DeleteAnswer(Guid id, Guid answerId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var answer = new Answer()
            {
                Id = answerId,
                UserId = user.Id
            };

            return Ok(await _answerService.DeleteAnswerAsync(id, answer));
        }



        [HttpPut]
        [Route("api/Question/Rating/{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult<int>> ChangeQRating(Guid id, bool mark)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            await _questionRatingService.CreateRating(currentUser, id, mark);

            return Ok(await _questionRatingService.GetQuestionRating(id));
        }

        [HttpDelete]
        [Route("api/Question/Rating/{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult<int>> DeleteQRating(Guid id)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            await _questionRatingService.DeleteRating(currentUser, id);

            return Ok(await _questionRatingService.GetQuestionRating(id));
        }


        [HttpPut]
        [Route("api/Answer/Rating/{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult<int>> ChangeARating(Guid id, bool mark)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            await _answerRatingService.CreateRating(currentUser, id, mark);

            return Ok(await _answerRatingService.GetAnswerRating(id));
        }


        [HttpDelete]
        [Route("api/Answer/Rating/{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult<int>> DeleteARating(Guid id)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            await _answerRatingService.DeleteRating(currentUser, id);

            return Ok(await _answerRatingService.GetAnswerRating(id));
        }
    }
}
