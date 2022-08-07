using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO.Course;

namespace DAL.DTO.Admin
{
    public class AdminShowUpdatedCourseDto : ShowUpdatedCourseDto
    {
        public int TeacherId { get; set; }


    }
}
