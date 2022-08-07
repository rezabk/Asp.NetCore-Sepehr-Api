using DAL.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BusinessLogic.Contracts
{
    public interface ICourseUserRepository
    {
        Task<ValueTask<EntityEntry<CourseUserModel>>> Add(CourseUserModel courseUserModel);

        CourseUserModel? CheckStudentForJoiningCourse(int studentId, int courseId);

        Task<bool> RemoveUserFromCourse(int studentid, int courseid);

        Task<List<CourseUserModel>> GetCourseStudents(int courseid, int teacherid);

        Task<List<CourseUserModel>> GetUserCourses(int userid);

        Task<bool> RemoveUser(int id);
        
        Task<bool> RemoveCourse(int id);


    }
}
