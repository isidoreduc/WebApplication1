using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.ViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class QuizController : Controller
    {
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
            var q = new QuizViewModel()
            {
                Id = id,
                Title = String.Format("Sample quiz with id: {0}", id),
                Description = "Not a real quiz, just a sample",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            };

            return new JsonResult(q, new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });
        }
        #endregion

        #region Attribute-based routing methods
        /// <summary>
        /// GET: api/quiz/latest
        /// Retrieves the {num} latest quizzes
        /// </summary>
        /// <param name="num"> The number of quizzes to return </param>
        /// <returns>{num} latest quizzes</returns>
        
        [HttpGet("Latest/{num}")]
        public IActionResult Latest(int num = 10)
        {
            var sampleQuizzes = new List<QuizViewModel>();
            
            // add a first sample quizz
            sampleQuizzes.Add(new QuizViewModel()
            {
                Id = 1,
                Title = "Which Harlots character are you?",
                Description ="Harlots tv series",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            });

            // add some few more quizzes
            for (int i = 2; i <= num; i++)
            {
                sampleQuizzes.Add(new QuizViewModel()
                {
                    Id = i,
                    Title = String.Format("Quizz {0}", i),
                    Description = "Sample quizz no. " + i,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                });
            }

            // output result in Json format
            return new JsonResult(sampleQuizzes, new Newtonsoft.Json.JsonSerializerSettings
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
            var sampleQuizzes = ((JsonResult)Latest(num)).Value 
                as List<QuizViewModel>;
            return new JsonResult(
                sampleQuizzes.OrderBy(t => t.Title),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                }
                );
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
            var sampleQuizzes = ((JsonResult)Latest(num)).Value
                as List<QuizViewModel>;
            return new JsonResult(
                sampleQuizzes.OrderBy(t => Guid.NewGuid()),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                }
                );
        }
        #endregion
    }
}
