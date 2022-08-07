using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.User
{
    public class ShowUserCoursesDto
    {
        public int Id { get; set; }

        public string CourseTitle { get; set; }

        public int Cost { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

    }
}
