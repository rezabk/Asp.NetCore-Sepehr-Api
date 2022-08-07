using DAL.DTO.Lesson;
using Data;

namespace BusinessLogic.BusinessLogics.Lesson
{
    public interface ILessonBl
    {

        Task<StandardResult> GetAllLessons();

        Task<StandardResult> GetLessonById(int id);

        Task<StandardResult> CreateLesson(CreateLessonDto dto);
        
        Task<StandardResult> UpdateLesson(UpdateLessonDto dto,int id);

        Task<StandardResult> Availability(int id);

        Task<StandardResult> RemoveLesson(int lessonId);


    }
}
