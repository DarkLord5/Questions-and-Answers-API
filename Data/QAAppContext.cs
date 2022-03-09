using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Questions_and_Answers_API.Models;

namespace Questions_and_Answers_API.Data
{
    public class QAAppContext : IdentityDbContext<User>
    {
        public QAAppContext(DbContextOptions<QAAppContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

        }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AnswerRating> AnswersRating { get; set;}
        public DbSet<QuestionRating> QuestionsRating { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnswerRating>().HasKey(u => new { u.UserId, u.AnswerId });
            modelBuilder.Entity<AnswerRating>().Property(u=> u.Mark).IsRequired();

            modelBuilder.Entity<QuestionRating>().HasKey(u=> new {u.UserId, u.QuestionId});
            modelBuilder.Entity<QuestionRating>().Property(u => u.Mark).IsRequired();
            modelBuilder.Entity<QuestionRating>().HasOne(a => a.Question).WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Tag>().Property(p=>p.Name).HasMaxLength(40).IsRequired();

            modelBuilder.Entity<Question>().Property(q => q.Title).HasMaxLength(40).IsRequired();
            modelBuilder.Entity<Question>().Property(q => q.Description).HasMaxLength(1000).IsRequired();
            
            

            modelBuilder.Entity<Answer>().Property(a=>a.Text).HasMaxLength(1000).IsRequired();
            modelBuilder.Entity<Answer>().HasOne(a => a.Question).WithMany().OnDelete(DeleteBehavior.NoAction);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
