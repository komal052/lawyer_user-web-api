using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practical_Test.Model
{
    public class Question
    {
       
        public int Id { get; set; }
        public string Description { get; set; }
        public int IsPicked { get; set; }


        public string image { get; set; }
        [NotMapped]
        public IFormFile images { get; set; }


        [ForeignKey(nameof(User))]
        public int UserID { get; set; }
        public User? User { get; set; }


        [ForeignKey(nameof(Lawyer))]
        public int? lawyerID { get; set; }
        public Lawyer? Lawyer { get; set; }
    }
}
