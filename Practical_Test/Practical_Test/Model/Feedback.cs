using System.ComponentModel.DataAnnotations.Schema;

namespace Practical_Test.Model
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int Rating { get; set; }

        [ForeignKey(nameof(User))]
        public int UserID { get; set; } 
        public User? User { get; set; }


        [ForeignKey(nameof(Lawyer))]
        public int LawyerID { get; set; }
        public Lawyer? Lawyer { get; set; }
    }
}
