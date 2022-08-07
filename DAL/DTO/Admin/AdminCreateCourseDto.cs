using DAL.DTO.Course;

namespace DAL.DTO.Admin
{
    public class AdminCreateCourseDto : CreateCourseDto
    {

        public int TeacherId { get; set; }

    }
}
