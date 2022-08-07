using DAL.Mapping;
using DAL.Model;
using DAL.Model.CourseModels;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AcademyDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<BecomeMasterModel> BecomeMaster { get; set; }

        public DbSet<LessonModel> Lessons { get; set; }
        public DbSet<CourseModel> Courses { get; set; }

        public DbSet<CourseUserModel> CourseUsers { get; set; }  
        
        public DbSet<SettingModel> Settings { get; set; }

        public AcademyDbContext(DbContextOptions dbContextOptions)
               : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LessonModel>().Property(x => x.LessonTopics).HasConversion(

                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)); 
            
          
            modelBuilder.ApplyConfiguration(new CourseMapping());
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new LessonMapping());
            modelBuilder.ApplyConfiguration(new CourseUserMapping());
           

            base.OnModelCreating(modelBuilder);
        }
    }
}