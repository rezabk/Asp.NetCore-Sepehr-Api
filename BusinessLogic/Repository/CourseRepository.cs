using System.Security.Cryptography.X509Certificates;
using BusinessLogic.Contracts;
using DAL;
using DAL.Model;
using DAL.Model.CourseModels;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AcademyDbContext _context;

        public CourseRepository(AcademyDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseModel>> GetAllCourses()
        {
            return await _context.Courses.Include(x => x.Lesson).Include(x => x.Teacher).Where(x => x.IsDeleted == false).ToListAsync();
        }

        public CourseModel GetCourseById(int id)
        {
            return _context.Courses.Include(x => x.Lesson).Include(x => x.Teacher).SingleOrDefault(x => x.Id == id && x.IsDeleted == false);
        }

        public UserModel GetTeacher(int id)
        {
            return _context.Users.SingleOrDefault(x => x.Id == id && x.Role == UserModel.UserRole.TEACHER && x.IsDeleted == false);
        }


        public LessonModel GetLesson(int id)
        {
            return _context.Lessons.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);
        }

        public async Task<CourseModel> CreateCourse(CourseModel courseModel)
        {
            await _context.Courses.AddAsync(courseModel);
            await _context.SaveChangesAsync();
            return courseModel;
        }

        public async Task<bool> RemoveCourse(int courseid)
        {
            var res = await _context.Courses.SingleOrDefaultAsync(x => x.Id == courseid);
            res.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CourseDeActiveOnLessonDeleting(int lessonId)
        {
            var res = await _context.Courses.Where(x => x.LessonId == lessonId).ToListAsync();
            foreach (var item in res)
            {
                item.IsActive = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }


    }
}
