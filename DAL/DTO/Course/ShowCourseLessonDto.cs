
namespace DAL.DTO.Course
{
    public class ShowCourseLessonDto
    {
        public int Id { get; set; }

        public string LessonName { get; set; }

        public string[] LessonTopics { get; set; }

        public string Description { get; set; }

        public ShowCourseLessonDto()
        {
            
        }

    }
}
