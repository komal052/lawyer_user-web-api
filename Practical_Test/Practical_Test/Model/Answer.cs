using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practical_Test.Model
{
    public class Answer
    {
        
        public int Id { get; set; }
        public string AnswerOfQuestion { get; set; }
        public int? IsSatisfied { get; set; }

         [ForeignKey(nameof(Question))]
        public int QuestionID { get; set; }
        public Question? Question { get; set; }

        [ForeignKey(nameof(Lawyer))]
        public int LawyerID { get; set; }
        public Lawyer? Lawyer { get; set; }
    }
}
