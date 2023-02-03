using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Practical_Test.Data;
using Practical_Test.Model;

namespace Practical_Test.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataDbContext _Context;
        private readonly ILogger<UserController> _logger;

        public UserController(DataDbContext Context, ILogger<UserController> logger)
        {
            _Context = Context;
            _logger = logger;
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<IEnumerable<User>>> AddUser(User u)
        {
            if (u == null)
            {
                return BadRequest("Record Not Added");
            }
            _Context.users.Add(u);
           
            await _Context.SaveChangesAsync();
            return Ok(u);
        }

        [HttpPost("AddQuestion")]
        public async Task<ActionResult<IEnumerable<Question>>> AddQuestion(string Question,int ispicked, int userId,int lawyerId,IFormFile file)
        {
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            cloudinary.Api.Secure = true;

            string FileName=file.FileName;

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + FileName;
                
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "D:/.net/upload/", FileName);

            file.CopyTo(new FileStream(imagePath, FileMode.Create));

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName.ToString(), file.OpenReadStream()),
                PublicId = "Demo"
              
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            var file1 = uploadResult.SecureUri.ToString();

            /*var que = _Context.questions.Where(x => x.lawyerID == lawyerId).First();
            if(que.IsPicked!=null && que.lawyerID!=null)
            {
                return Ok("question is already Picked!");
            }*/

            Question q = new Question();
            q.Description = Question;
            q.IsPicked = ispicked;
            q. lawyerID= lawyerId;
            q.UserID= userId;
            q.image = file1;
            if (q == null)
            {
                return BadRequest("record Not added");
            }
            _logger.LogInformation("Add Question");
            _Context.questions.Add(q);
            await _Context.SaveChangesAsync();
            return Ok(q);
        }

        [HttpPost("AddFeedback")]
        public async Task<ActionResult<IEnumerable<Feedback>>> AddFeedback(Feedback fb)
        {
            if(fb == null)
            {
                return BadRequest("Your Feedback not Added");
            }
            _Context.feedbacks.Add(fb);
            await _Context.SaveChangesAsync();
            return Ok(fb);
        }

        //reassign the lawyer
        [HttpPut("UpdateLawyer")]
        public async Task<ActionResult<IEnumerable<Answer>>> UpdateLawyer(Answer ans)
        {
          
            var ul = await _Context.answers.FindAsync(ans.Id);
            if(ul== null)
            {
                return NotFound("Record couldn't  found");
            }

            ul.QuestionID = ans.QuestionID;
            ul.LawyerID = ans.LawyerID;
             ul.IsSatisfied = 0;
            ul.AnswerOfQuestion = ans.AnswerOfQuestion;
          
            _Context.Entry(ul).State = EntityState.Modified;
            await _Context.SaveChangesAsync();

            var que = _Context.questions.Where(x=>x.Id == ans.QuestionID).First();
            que.lawyerID = ans.LawyerID;
            
            _Context.Entry(que).State = EntityState.Modified;
            await _Context.SaveChangesAsync();

            return Ok(ul);


        }

        //history 
        [HttpGet("HistoryOfQuestionAnswer")]

        public IActionResult GetAnswer(int userid)
        {
            
           
            var r = _Context.answers.Where(x => x.Question.User.Id == userid).
                Where(x => x.Question.IsPicked == 1).
               Join(_Context.questions,
                ans => ans.QuestionID, que => que.Id,
                (ans, que) => new
                {
                    answer = ans.AnswerOfQuestion,
                    question = que.Description
                });

           
           
           /* var r = _Context.answers.Where(x => x.Question.User.Id == userid).
                Where(x => x.Question.IsPicked == 1).   
                Include(data=>data.Question.Description).ToList();*/
            /*   Include(data => data.Question.Lawyer.lawyerName).*/

            /*if(r==null)
            {
                return
                    BadRequest("record Not Found");
            }*/

            return Ok(r);

        }
        [HttpPut("UpdateSatisfied")]
        public async Task<ActionResult<IEnumerable<Answer>>> UpdateSatisfied(int id,string answer,int questionId,int lawyerId)
        {
          
            var ul = await _Context.answers.FindAsync(id);
            if(ul.Id!=id)
            {
                return BadRequest("id Not found");
            }
            ul.QuestionID = questionId;
            ul.LawyerID = lawyerId;
         
            ul.IsSatisfied = 1;
            ul.AnswerOfQuestion = answer;



           
            await _Context.SaveChangesAsync();
            return Ok(ul);
        }


    }
}
