using Microsoft.EntityFrameworkCore;

namespace MagneticSurvey.Data
{
    public class SurveyDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<QuestionEntity> Questions { get; set; }
        public DbSet<UserQuestion> UserQuestion { get; set; }
        public SurveyDbContext(DbContextOptions<SurveyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserQuestion>()
          .HasKey(o => new { o.QuestionEntitysId, o.UserEntitysId });

            modelBuilder.Entity<UserQuestion>()
    .HasOne(pt => pt.UserEntity)
    .WithMany(p => p.UserQuestions)
    .HasForeignKey(pt => pt.UserEntitysId);

            modelBuilder.Entity<UserQuestion>()
.HasOne(pt => pt.QuestionEntity)
.WithMany(p => p.UserQuestions)
.HasForeignKey(pt => pt.QuestionEntitysId);

        }
    }
}
