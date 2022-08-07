using BusinessLogic.Contracts;
using DAL;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BusinessLogic.Repository
{
    public class CourseUserRepository : ICourseUserRepository
    {
        private readonly AcademyDbContext _context;

        public CourseUserRepository(AcademyDbContext context)
        {
            _context = context;
        }



        public async Task<ValueTask<EntityEntry<CourseUserModel>>> Add(CourseUserModel courseUserModel)
        {
            await _context.CourseUsers.AddAsync(courseUserModel);
            await _context.SaveChangesAsync();
            return new ValueTask<EntityEntry<CourseUserModel>>();
        }

        public CourseUserModel? CheckStudentForJoiningCourse(int studentId, int courseId)
        {
            return _context.CourseUsers.SingleOrDefault(x => x.CourseId == courseId && x.UserId == studentId);

        }

        public async Task<bool> RemoveUserFromCourse(int studentid, int courseid)
        {
            var res = _context.CourseUsers.SingleOrDefault(x => x.UserId == studentid && x.CourseId == courseid);
            _context.CourseUsers.Remove(res);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<List<CourseUserModel>> GetCourseStudents(int courseid, int teacherid)
        {
            return await _context.CourseUsers.Include(x => x.User).Where(x => x.UserId != teacherid).ToListAsync();
        }

        public async Task<List<CourseUserModel>> GetUserCourses(int userid)
        {
            return await _context.CourseUsers.Include(x => x.Course).Where(x => x.UserId == userid).ToListAsync();
        }

        public async Task<bool> RemoveUser(int id)
        {
            var res = await _context.CourseUsers.Where(x => x.UserId == id).ToListAsync();
            _context.RemoveRange(res);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveCourse(int id)
        {
            var res = await _context.CourseUsers.Where(x => x.CourseId == id).ToListAsync();
            _context.RemoveRange(res);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
