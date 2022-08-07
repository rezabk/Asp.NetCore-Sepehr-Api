

namespace DAL.DTO.Course
{
    public class ShowCoursesDto
    {

        public int Id { get; set; }

        public string CourseTitle { get; set; }

        public int Cost { get; set; }

        public ShowCourseUsersDto TeacherDetails { get; set; }

        public ShowCourseLessonDto LessonDetails { get; set; }

        public List<ShowCourseUsersDto> StudentsDetails { get; set; }

    
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string CreationDate { get; set; }

        public int Capacity { get; set; }

        public bool IsActive { get; set; }



    }
}
