using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DAL.Model.CourseModels;

namespace DAL.Model
{
    public class LessonModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string LessonName { get; set; }

        [Required]
        [MaxLength(255)]
        public string[] LessonTopics { get; set; }

        [AllowNull]
        [MaxLength(255)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(255)]
        public string CreationDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsDeleted { get; set; }


        public List<CourseModel> Courses { get; set; }


        public LessonModel()
        {
            Courses = new List<CourseModel>();
            IsDeleted = false;
            IsActive = true;
        }


    }
}
