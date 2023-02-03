using Microsoft.EntityFrameworkCore;
using Practical_Test.Model;

namespace Practical_Test.Data
{
    public class DataDbContext :DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options):base(options)
        {

        }

        public DbSet<User> users { get; set; }
        public DbSet<Lawyer> lawyers { get; set; }
        public DbSet<Question> questions { get; set; }
        public DbSet<Feedback> feedbacks { get; set; }
        public DbSet<Answer> answers { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>().HasOne(q=>q.Questions).WithMany(a=>a.a)

        }*/


    }
   



}
