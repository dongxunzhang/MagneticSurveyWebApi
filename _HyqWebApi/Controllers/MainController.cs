using MagneticSurvey.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Xml.Linq;

namespace _HyqWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {

        private readonly ILogger<MainController> _logger;
        private readonly SurveyDbContext _context;

        public MainController(ILogger<MainController> logger, SurveyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("Login")]
        public async Task<int> Login(string name, string pwd)
        {
            var model = await _context.Users.FirstOrDefaultAsync(m => m.Email == name && m.Pwd == pwd);
            if (model != null)
            {
                return model.Id;
            }
            return 0;
        }

        [HttpGet("Register")]
        public async Task<int> Register(string email, string pwd, string username, int id)
        {
            try
            {
                UserEntity user;
                if (id > 0)
                {
                    user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
                }
                else
                {
                    user = new UserEntity();
                    _context.Users.Add(user);
                }

                user.Email = email;
                user.Pwd = pwd;
                user.UserName = username;

                await _context.SaveChangesAsync();

                return 1;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return 0;
            }
        }

        [HttpGet("GetUsers")]
        public async Task<List<UserEntity>> GetUsers()
        {
            try
            {
                var list = await _context.Users.ToListAsync();

                return list;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new List<UserEntity>();
            }
        }

        [HttpGet("Del")]
        public async Task<int> Del(int id)
        {
            try
            {
                var model = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
                if (model != null)
                {
                    _context.Users.Remove(model);
                    await _context.SaveChangesAsync();

                    return 1;
                }

                return 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return 0;
            }
        }

        [HttpGet("DelQt")]
        public async Task<int> DelQt(int id)
        {
            try
            {
                var model = await _context.Questions.FirstOrDefaultAsync(m => m.Id == id);
                if (model != null)
                {
                    _context.Questions.Remove(model);
                    await _context.SaveChangesAsync();

                    return 1;
                }

                return 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return 0;
            }
        }

        [HttpGet("GetQuestions")]
        public async Task<List<QuestionEntity>> GetQuestions()
        {
            try
            {
                var list = await _context.Questions.ToListAsync();

                return list;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new List<QuestionEntity>();
            }
        }

        [HttpGet("InitUserQuestions")]
        public async Task<int> InitUserQuestions(int id)
        {
            try
            {
                var questions = _context.Questions
        .Include(m => m.UserQuestions)
        .Where(m => m.UserQuestions.Where(m => m.UserEntitysId == id).Count() == 0)
        .ToList();

                foreach (var item in questions)
                {
                    int i = await _context.UserQuestion.Where(m => m.QuestionEntitysId == item.Id).CountAsync();
                    if (i == 0)
                    {
                        UserQuestion question = new UserQuestion();
                        question.UserEntitysId = id;
                        question.QuestionEntitysId = item.Id;
                        item.UserQuestions.Add(question);
                    }
                }

                await _context.SaveChangesAsync();

                return 1;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return 0;
            }
        }

        [HttpGet("NextUserQuestions")]
        public async Task<object> NextUserQuestions(int id)
        {
            try
            {
                var firstQuestion = await _context.UserQuestion.Include(m => m.QuestionEntity).Where(m => m.UserEntitysId == id && !m.IsRead).FirstOrDefaultAsync();
                if (firstQuestion != null)
                {
                    firstQuestion.IsRead = true;

                    await _context.SaveChangesAsync();


                    return new { content = firstQuestion.QuestionEntity.Content, id = firstQuestion.QuestionEntity.Id, type = firstQuestion.QuestionEntity.Type };
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return new object();
        }

        [HttpGet("AddQuestions")]
        public async Task<int> AddQuestions(string content, string type, int id)
        {
            try
            {
                QuestionEntity model;
                if (id > 0)
                {
                    model = await _context.Questions.FirstOrDefaultAsync(m => m.Id == id);
                }
                else
                {
                    model = new QuestionEntity();
                    _context.Questions.Add(model);
                }

                model.Content = content;
                model.Type = type;

                await _context.SaveChangesAsync();

                return 1;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return 0;
            }
        }
    }
}