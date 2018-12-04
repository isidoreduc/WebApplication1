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
    public class QuestionController : Controller
    {
        #region Private Fields
        private ApplicationDbContext _dbContext;
        #endregion

        #region Constructor
        public QuestionController(ApplicationDbContext context)
        {
            // Instantiate the Application_dbContext through DI
            _dbContext = context;
        }
        #endregion

        #region RESTful conventions methods
        #region Get Method
        /// <summary>
        /// Retrieves the Question with the given {id}
        /// </summary>
        /// <param name="id">The ID of an existing Question</param>
        /// <returns>the Question with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question = _dbContext.Questions.Where(i => i.Id == id)
                .FirstOrDefault();
            // handle requests asking for non-existing questions
            if (question == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Question ID {0} has not been found", id)
                });
            }
            return new JsonResult(
                question.Adapt<QuestionViewModel>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });
        }
        #endregion M
        #region Post Method
        
        /// <summary>
        /// Adds a new Question to the Database
        /// </summary>
        /// <param name="model">The QuestionViewModel containing the data to insert</param>
        [HttpPost]
        public IActionResult Post([FromBody]QuestionViewModel model)
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (model == null) return new StatusCodeResult(500);
            // map the ViewModel to the Model
            var question = model.Adapt<Question>();
            // override those properties
            //   that should be set from the server-side only
            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;
            // properties set from server-side
            question.CreatedDate = DateTime.Now;
            question.LastModifiedDate = question.CreatedDate;
            // add the new question
            _dbContext.Questions.Add(question);
            // persist the changes into the Database.
            _dbContext.SaveChanges();
            // return the newly-created Question to the client.
            return new JsonResult(question.Adapt<QuestionViewModel>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });
        }
        #endregion
        #region Put Method
        /// <summary>
        /// Edit the Question with the given {id}
        /// </summary>
        /// <param name="model">The QuestionViewModel containing the data to update</param>
        [HttpPut]
        public IActionResult Put([FromBody]QuestionViewModel model)
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (model == null) return new StatusCodeResult(500);
            // retrieve the question to edit
            var question = _dbContext.Questions.Where(q => q.Id ==
                        model.Id).FirstOrDefault();
            // handle requests asking for non-existing questions
            if (question == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Question ID {0} has not been found", model.Id)
                });
            }
            // handle the update (without object-mapping)
            //   by manually assigning the properties
            //   we want to accept from the request
            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;
            // properties set from server-side
            question.LastModifiedDate = question.CreatedDate;
            // persist the changes into the Database.
            _dbContext.SaveChanges();
            // return the updated Quiz to the client.
            return new JsonResult(question.Adapt<QuestionViewModel>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });
        }
        #endregion
        #region Delete Method
        /// <summary>
        /// Deletes the Question with the given {id} from the Database
        /// </summary>
        /// <param name="id">The ID of an existing Question</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // retrieve the question from the Database
            var question = _dbContext.Questions.Where(i => i.Id == id)
                .FirstOrDefault();
            // handle requests asking for non-existing questions
            if (question == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Question ID {0} has not been found", id)
                });
            }
            // remove the quiz from the _dbContext.
            _dbContext.Questions.Remove(question);
            // persist the changes into the Database.
            _dbContext.SaveChanges();
            // return an HTTP Status 200 (OK).
            return new OkResult();
        }
        #endregion
        #endregion

        // GET: api/question/all - get all questions of a quiz
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var questions = _dbContext.Questions.Where(question => question.Id == quizId).ToArray();
            return new JsonResult(
                questions.Adapt<QuestionViewModel[]>(),
                new Newtonsoft.Json.JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented
                });
        }

    }
}
