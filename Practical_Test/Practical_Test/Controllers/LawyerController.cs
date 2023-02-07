using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practical_Test.Data;
using Practical_Test.Model;
using System.Linq;

namespace Practical_Test.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LawyerController : ControllerBase
    {
        private readonly DataDbContext _Context;

        public LawyerController(DataDbContext Context)
        {
            _Context = Context;
        }

       
        [HttpPost("AddLawyer")]
        public async Task<ActionResult<IEnumerable<Lawyer>>> AddLawyer(Lawyer l)
        {
            
            if (l == null)
            {
                return NotFound();
            }
            _Context.lawyers.Add(l);
            await _Context.SaveChangesAsync();
            return Ok(l);
        }
        //able to pick question ans  
        [HttpPost("AddAnswer")]
        public async Task<ActionResult<IEnumerable<Answer>>> AddAnswer(string answer,int questionId,int lawyerId)
        {
            Answer ans = new Answer();
           
            /*  var res = ans.IsSatisfied == 0 ? "not confirm" : ans.IsSatisfied == 1 ? "satisfied" : "Not satisfied";*/
            ans.AnswerOfQuestion = answer;
            ans.IsSatisfied= null;
            ans.QuestionID = questionId;
            ans.LawyerID = lawyerId;
           
            
            _Context.answers.Add(ans);
            await _Context.SaveChangesAsync();
            return Ok(ans);
        }

        //not picked any question and not assign
        [HttpGet("GetQuestionNotpickedOrAssign")]

        public IActionResult GetQuestion()
        {
          
            var r =
                  _Context.questions.Where(x => x.IsPicked == 0 && x.lawyerID == null)
                   .Join(
                    _Context.users,
                    que=>que.UserID,
                    user => user.Id,
                    (que, user) => new
                    {
                        question=que.Description,

                    });

            if (r == null)
            {
                return NotFound();
            }
            return Ok(r);

        }

        [HttpGet("GETAverageRating")]
        public IActionResult GETAverageRating(int LawyerId)
        {
           
            var avg = _Context.feedbacks.Where(data => data.Lawyer.Id == LawyerId)
                .Average(data =>data.Rating );
            return Ok("Average Rating :" + avg);
        }


    }
}
