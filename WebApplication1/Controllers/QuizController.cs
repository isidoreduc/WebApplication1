using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Data;
using WebApplication1.ViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class QuizController : Controller
    {
        #region Private Fields
        private ApplicationDbContext _dbContext;
        #endregion
       
        #region Constructor
        public QuizController(ApplicationDbContext context)
        {
            // Instantiate the ApplicationDbContext through DI
            _dbContext = context;
        }
        #endregion Constructor

        #region RESTful conventions routing
        /// <summary>
        /// GET: api/quiz/{id}
        /// Retrieves the quiz with the given id
        /// </summary>
        /// <param name="id"> The id of wanted quiz </param>
        /// <returns> The quiz with the given id </returns>

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // create a sample quiz 
            var quiz = _dbContext.Quizzes.Where(i => i.Id == id).FirstOrDefault();
            // handle requests asking for non-existing quizzes
            if (quiz == null) 
                return NotFound(new
                {
                    Error = String.Format("Quiz ID {0} has not been found", id)
                });
            

            return new JsonResult(
                quiz.Adapt<QuizViewModel>(), // using Mapster (faster than AutoMapper) to map quiz properties to the QuizViewModel
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });
        }

        /// <summary>
        /// Adds a new Quiz to the Database
        /// </summary>
        /// <param name="model">The QuizViewModel containing the data to insert</param>
        [HttpPost]
        public IActionResult Post([FromBody] QuizViewModel model)
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (model == null) return new StatusCodeResult(500);
            // handle the insert (without object-mapping)
            var quiz = new Quiz();
            // properties taken from the request
            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes;
            // properties set from server-side
            quiz.CreatedDate = DateTime.Now;
            quiz.LastModifiedDate = quiz.CreatedDate;
            // Set a temporary author using the Admin user's userId
            // as user login isn't supported yet: we'll change this later on.
            quiz.UserId = _dbContext.Users.Where(u => u.UserName == "Admin")
                .FirstOrDefault().Id;
            // add the new quiz
            _dbContext.Quizzes.Add(quiz);
            // persist the changes into the Database.
            _dbContext.SaveChanges();
            // return the newly-created Quiz to the client.
            return new JsonResult(quiz.Adapt<QuizViewModel>(),
                        new JsonSerializerSettings()
                        {
                            Formatting = Formatting.Indented
                        });
        }

        /// <summary>
        /// Edit the Quiz with the given {id}
        /// </summary>
        /// <param name="model">The QuizViewModel containing the data to update</param>
        [HttpPut]
        public IActionResult Put([FromBody]QuizViewModel model) // FromBody - tell .NET Core to fetch the QuizViewModel sent by our
                                                                // Angular app--in JSON format--from the request body
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (model == null) return new StatusCodeResult(500);
            // retrieve the quiz to edit
            var quiz = _dbContext.Quizzes.Where(q => q.Id ==
                model.Id).FirstOrDefault();
            // handle requests asking for non-existing quizzes
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Quiz ID {0} has not been found",
                         model.Id)
                });
            }
            // handle the update (without object-mapping)
            //   by manually assigning the properties
            //   we want to accept from the request
            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes;
            // properties set from server-side
            quiz.LastModifiedDate = quiz.CreatedDate;
            // persist the changes into the Database.
            _dbContext.SaveChanges();
            // return the updated Quiz to the client.
            return new JsonResult(quiz.Adapt<QuizViewModel>(), //return a new QuizViewModel to the client built upon the created Quiz
                        new JsonSerializerSettings()
                        {
                            Formatting = Formatting.Indented
                        });
        }

        /// <summary>
        /// Deletes the Quiz with the given {id} from the Database
        /// </summary>
        /// <param name="id">The ID of an existing Test</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // retrieve the quiz from the Database
            var quiz = _dbContext.Quizzes.Where(i => i.Id == id)
                .FirstOrDefault();
            // handle requests asking for non-existing quizzes
            if (quiz == null)
                return NotFound(new
                {
                    Error = String.Format("Quiz ID {0} has not been found", id)
                });
            // remove the quiz from the DbContext.
            _dbContext.Quizzes.Remove(quiz);
            // persist the changes into the Database.
            _dbContext.SaveChanges();
            // return an HTTP Status 200 (OK).
            return new OkResult();
        }

    #endregion

    #region Attribute-based routing methods
    /// <summary>
    /// GET: api/quiz/latest
    /// Retrieves the {num} latest quizzes
    /// </summary>
    /// <param name="num"> The number of quizzes to return </param>
    /// <returns>{num} latest quizzes</returns>

    [HttpGet("Latest/{num?}")] // num is optional, nullable
        public IActionResult Latest(int num = 10)
        {
            //var sampleQuizzes = new List<QuizViewModel>();

            //// add a first sample quizz
            //sampleQuizzes.Add(new QuizViewModel()
            //{
            //    Id = 1,
            //    Title = "Which Harlots character are you?",
            //    Description ="Harlots tv series",
            //    CreatedDate = DateTime.Now,
            //    LastModifiedDate = DateTime.Now
            //});

            //// add some few more quizzes
            //for (int i = 2; i <= num; i++)
            //{
            //    sampleQuizzes.Add(new QuizViewModel()
            //    {
            //        Id = i,
            //        Title = String.Format("Quizz {0}", i),
            //        Description = "Sample quizz no. " + i,
            //        CreatedDate = DateTime.Now,
            //        LastModifiedDate = DateTime.Now
            //    });
            //}
            var latest = _dbContext.Quizzes
                .OrderByDescending(x => x.CreatedDate)
                .Take(num)
                .ToArray();
            // output result in Json format
            return new JsonResult(
                latest.Adapt<QuizViewModel[]>(), 
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
        }

        
         /// <summary>
         ///   GET: api/quiz/ByTitle
         ///   Retrieves the {num} Quizzes sorted alphabetically
         /// </summary>
         /// <param name = "num"> 
         ///  The number of quizzes to retrieve
         /// </param>
         ///<returns>
         /// {num} Quizzes sorted by Title
         /// </returns>     
        
        [HttpGet("ByTitle/{num:int?}")]
        public IActionResult ByTitle(int num = 10)
        {
            //var sampleQuizzes = ((JsonResult)Latest(num)).Value 
            //    as List<QuizViewModel>;
            //return new JsonResult(
            //    sampleQuizzes.OrderBy(t => t.Title),
            //    new JsonSerializerSettings()
            //    {
            //        Formatting = Formatting.Indented
            //    }
            //    );
            var byTitle = _dbContext.Quizzes
                .OrderBy(q => q.Title)
                .Take(num)
                .ToArray();
            return new JsonResult(
                byTitle.Adapt<QuizViewModel[]>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });
        }

        /*
         <summary>
            GET: api/quiz/mostViewed
            Retrieves the {num} random Quizzes 
        </summary>
        <param name = "num"> 
            The number of quizzes to retrieve
        </param>
        <returns>
            {num} random Quizzes 
        </returns>     
        */
        [HttpGet("Random/{num:int?}")]
        public IActionResult Random(int num = 10)
        {
            //var sampleQuizzes = ((JsonResult)Latest(num)).Value
            //    as List<QuizViewModel>;
            //return new JsonResult(
            //    sampleQuizzes.OrderBy(t => Guid.NewGuid()),
            var random = _dbContext.Quizzes
                .OrderBy(q => Guid.NewGuid())
                .Take(num)
                .ToArray();
            return new JsonResult(
                random.Adapt<QuizViewModel[]>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                }
                );
        }
        #endregion
    }
}
