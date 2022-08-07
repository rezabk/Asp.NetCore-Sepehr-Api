using System.ComponentModel.DataAnnotations;
using DAL.Model.CourseModels;

namespace DAL.Model
{
    public class CourseUserModel
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public CourseModel Course { get; set; }

        public int UserId { get; set; }

        public UserModel User { get; set; }

        public bool IsDeleted { get; set; }

        public CourseUserModel()
        {
            IsDeleted = false;
        }

    }
}
