using DAL.Model;

namespace BusinessLogic.Contracts
{
    public interface ILessonRepository
    {
        Task<List<LessonModel>> GetAllLessons();
        LessonModel GetLessonById(int id);
       
        Task<LessonModel> AddLesson(LessonModel lessonModel);

        Task<bool> CheckLessonName(string lessonName);

        Task<bool> RemoveLesson(int lessonId);

       
        void Save();
    }
}
