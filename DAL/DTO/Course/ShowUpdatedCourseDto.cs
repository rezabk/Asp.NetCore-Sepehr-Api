using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Course
{
    public class ShowUpdatedCourseDto
    {
        public string CourseTitle { get; set; }

        public int Cost { get; set; }

        public int LessonId { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int Capacity { get; set; }
    }
}
