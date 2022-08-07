using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Course
{
    public class ShowCreatedCourseDto
    {
        public int Id { get; set; }

        public string CourseTitle { get; set; }

        public int Cost { get; set; }

        public ShowCourseUsersDto TeacherDetails { get; set; }

        public ShowCourseLessonDto LessonDetails { get; set; }

      
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string CreationDate { get; set; }

        public int Capacity { get; set; }

      
    }
}
