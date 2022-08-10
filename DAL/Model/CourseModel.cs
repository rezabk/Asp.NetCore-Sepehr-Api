using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DAL.Model.CourseModels
{
    public class CourseModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string CourseTitle { get; set; }

        [Required]
        [MaxLength(255)]
        public int Cost { get; set; }

        public int TeacherId { get; set; }

        [Required]
        public UserModel Teacher { get; set; }

        [Required]
        public int LessonId { get; set; }

        [Required]
        public LessonModel Lesson { get; set; }


        public List<UserModel> Students { get; set; }


        [Required]
        [MaxLength(255)]
        public string StartDate { get; set; }

        [Required]
        [MaxLength(255)]
        public string EndDate { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public string CreationDate { get; set; }

        [AllowNull]
        public string? ImagePath { get; set; }
        [AllowNull]
        public string? ImageName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsDeleted { get; set; }


        public List<CourseUserModel> CourseUser { get; set; }

        public CourseModel()
        {
            Students = new List<UserModel>();
            CourseUser = new List<CourseUserModel>();
            IsDeleted = false;
            IsActive = true;
        }

    }

}


