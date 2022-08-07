
namespace DAL.DTO.Course
{
    public class CreateCourseDto
    {
        public string CourseTitle { get; set; }

        public int Cost { get; set; }

        public int LessonId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Capacity { get; set; }

    }
}
