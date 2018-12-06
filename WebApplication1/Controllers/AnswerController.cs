﻿using System;
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
    public class AnswerController : BaseApiController
    {
        
        #region Constructor
        public AnswerController(ApplicationDbContext context) : base(context) { }
        #endregion

        #region RESTful conventions methods
        #region Get
        /// <summary>
        /// Retrieves the Answer with the given {id}
        /// </summary>
        /// <param name="id">The ID of an existing Answer</param>
        /// <returns>the Answer with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var answer = DbContext.Answers.Where(i => i.Id == id)
                .FirstOrDefault();
            // handle requests asking for non-existing answers
            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Answer ID {0} has not been found", id)
                });
            }
            return new JsonResult(
                answer.Adapt<AnswerViewModel>(),
                JsonSettings);
        }
        #endregion
        #region Post
        // <summary>
        /// Adds a new Answer to the Database
        /// </summary>
        /// <param name="model">The AnswerViewModel containing the data to insert</param>

        [HttpPost]
        public IActionResult Post([FromBody]AnswerViewModel model)
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (model == null) return new StatusCodeResult(500);
            // map the ViewModel to the Model
            var answer = model.Adapt<Answer>();
            // override those properties
            //   that should be set from the server-side only
            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Notes = model.Notes;
            // properties set from server-side
            answer.CreatedDate = DateTime.Now;
            answer.LastModifiedDate = answer.CreatedDate;
            // add the new answer
            DbContext.Answers.Add(answer);
            // persist the changes into the Database.
            DbContext.SaveChanges();
            // return the newly - created Answer to the client.
            return new JsonResult(
                answer.Adapt<AnswerViewModel>(),
                JsonSettings);
        }
        #endregion
        #region Put
        /// <summary>
        /// Edit the Answer with the given {id}
        /// </summary>
        /// <param name="model">The AnswerViewModel containing the data  to update</param>

        [HttpPut]
        public IActionResult Put([FromBody]AnswerViewModel model)
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (model == null) return new StatusCodeResult(500);
            // retrieve the answer to edit
            var answer = DbContext.Answers.Where(q => q.Id ==
                        model.Id).FirstOrDefault();
            // handle requests asking for non-existing answers
            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Answer ID {0} has not been found", model.Id)
                });
            }
            // handle the update (without object-mapping)
            //   by manually assigning the properties
            //   we want to accept from the request
            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Value = model.Value;
            answer.Notes = model.Notes;
            // properties set from server-side
            answer.LastModifiedDate = answer.CreatedDate;
            // persist the changes into the Database.
            DbContext.SaveChanges();
            // return the updated Quiz to the client.
            return new JsonResult(
                answer.Adapt<AnswerViewModel>(),
                JsonSettings);
        }
        #endregion
        #region Delete
        /// <summary>
        /// Deletes the Answer with the given {id} from the Database
        /// </summary>
        /// <param name="id">The ID of an existing Answer</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // retrieve the answer from the Database
            var answer = DbContext.Answers.Where(i => i.Id == id)
                .FirstOrDefault();
            // handle requests asking for non-existing answers
            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Answer ID {0} has not been found", id)
                });
            }
            // remove the quiz from the DbContext.
            DbContext.Answers.Remove(answer);
            // persist the changes into the Database.
            DbContext.SaveChanges();
            // return an HTTP Status 200 (OK).
            return new OkResult();
        }
        #endregion
        #endregion


        // GET: api/answer/all
        [HttpGet("All/{questionId}")]
        public IActionResult All(int questionId)
        {
            var answers = DbContext.Answers.Where(a => a.Id == questionId).ToArray();
            return new JsonResult(
                answers.Adapt<AnswerViewModel[]>(),
                JsonSettings);
        }

        
    }
}
