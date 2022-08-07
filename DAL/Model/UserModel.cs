using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DAL.Model.CourseModels;


namespace DAL.Model
{
    public class UserModel
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "لطفا نام خود را وارد کنید")]
        [MinLength(3, ErrorMessage = "نام نمی توان کمتر از 3 کاراکتر باشد")]
        [MaxLength(255, ErrorMessage = "نام نمی تواند بیش از 255 کاراتر باشد")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "لطفا نام خانوادگی خود را وارد کنید")]
        [MinLength(3, ErrorMessage = "نام خانوادگی نمی توان کمتر از 3 کاراکتر باشد")]
        [MaxLength(255, ErrorMessage = "نام خانوادگی نمی تواند بیش از 255 کاراتر باشد")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "لطفا رمز عبور خود را وارد کنید")]
        [MinLength(8, ErrorMessage = "پسوورد نمی تواند کمتر از 8 کاراکتر باشد")]
        [MaxLength(1000)]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا ایمیل خود را وارد کنید")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده صحیح نیست")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا شماره موبایل خود را وارد کنید")]
        [MinLength(11, ErrorMessage = "شماره موبایل نمی تواند کمتر از 11 کاراکتر باشد")]
        [MaxLength(13, ErrorMessage = "شماره موبایل نمی تواند از 13 کاراکتر بیشتر باشد")]
        public string PhoneNumber { get; set; }

        [AllowNull]
        [MaxLength(500, ErrorMessage = "آدرس نمی تواند بیش از 500 کاراتر باشد")]
        public string? Address { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public List<CourseModel> Courses { get; set; }

        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        [AllowNull]
        [MaxLength(500, ErrorMessage = "آدرس نمی تواند بیش از 500 کاراتر باشد")]
        public string? RefreshTokenHash { get; set; }
        [AllowNull]
        public DateTime? RefreshTokenExTime { get; set; }

        public List<CourseUserModel> CourseUser { get; set; }

        public UserModel()
        {
            CourseUser = new List<CourseUserModel>();
            Courses = new List<CourseModel>();
            IsDeleted = false;
            RefreshTokenHash = "";
        }

        public void Refresh(string refreshTokenHash, DateTime exp)
        {
            RefreshTokenHash = refreshTokenHash;
            RefreshTokenExTime = exp;
        }

        public bool ValidateRefreshToken(string token)
        {
            return RefreshTokenHash.Equals(token);
        }
        public enum UserRole
        {
            ADMIN,

            TEACHER,

            STUDENT
        }



    }
}
