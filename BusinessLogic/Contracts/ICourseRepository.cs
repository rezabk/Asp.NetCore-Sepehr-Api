using DAL.Model;
using DAL.Model.CourseModels;


namespace BusinessLogic.Contracts
{
    public interface ICourseRepository
    {

        Task<List<CourseModel>> GetAllCourses();

        CourseModel GetCourseById(int id);

        UserModel GetTeacher(int id);

        LessonModel GetLesson(int id);
        
        Task<CourseModel> CreateCourse(CourseModel courseModel);

        Task<bool> RemoveCourse(int courseid);

        Task<bool> CourseDeActiveOnLessonDeleting(int lessonId);
 
        void Save();

      

    }
}
