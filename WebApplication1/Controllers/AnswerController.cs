using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.ViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class AnswerController : Controller
    {
        
        // GET: api/answer/all
        [HttpGet("All/{questionId}")]
        public IActionResult All(int questionId)
        {
            var sampleAnswers = new List<AnswerViewModel>();

            // add a first sample question
            sampleAnswers.Add(new AnswerViewModel()
            {
                Id = 1,
                QuestionId = questionId,
                Text = "Candy and a little brother",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            });

            // add some more questions
            for (int i = 2; i <= 5; i++)
            {
                sampleAnswers.Add(new AnswerViewModel()
                {
                    Id = i,
                    QuestionId = questionId,
                    Text = String.Format("Sample answer {0}", i),
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                });
            }
            return new JsonResult(sampleAnswers,
                new Newtonsoft.Json.JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented
                });
        }

        #region RESTful conventions routing
        /// <summary>
        /// Gets the answer with the given {id}
        /// </summary>
        /// <param name="id"> The id of an existing answer </param>
        /// <returns> Answer with given id </returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Content("not implemented!");
        }

        /// <summary>
        /// Adds answer to database
        /// </summary>
        /// <param name="a"> The AnswerViewModel containing the data to insert</param>
        [HttpPut]
        public IActionResult Put(AnswerViewModel a)
        {
            throw new NotImplementedException(); // use it when no return 
        }

        /// <summary>
        /// Edit answer to database
        /// </summary>
        /// <param name="a"> The AnswerViewModel containing the data to insert</param>
        [HttpPost]
        public IActionResult Post(AnswerViewModel a)
        {
            throw new NotImplementedException(); // use it when no return 
        }

        /// <summary>
        /// Delete answer from database
        /// </summary>
        /// <param name="id"> The answer to delete </param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException(); // use it when no return 
        }


        #endregion
    }
}
