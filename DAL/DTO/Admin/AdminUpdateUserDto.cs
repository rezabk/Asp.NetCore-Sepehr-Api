using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DAL.Model;

namespace DAL.DTO
{
    public class AdminUpdateUserDto
    {


        [Required(ErrorMessage = "لطفا نام کاربر را وارد کنید")]
        [MinLength(3, ErrorMessage = "نام کاربر نمی تواند کمتر از 3 کاراکتر باشد")]
        [MaxLength(255, ErrorMessage = "نام کاربر نمی تواند بیش از 255 کاراتر باشد")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "لطفا نام خانوادگی کاربر را وارد کنید")]
        [MinLength(3, ErrorMessage = "نام خانوادگی کاربر نمی تواند کمتر از 3 کاراکتر باشد")]
        [MaxLength(255, ErrorMessage = "نام خانوادگی کاربر نمی تواند بیش از 255 کاراتر باشد")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "لطفا رمز عبور کاربر را وارد کنید")]
        [MinLength(8, ErrorMessage = "رمز عبور نمی تواند کمتر از 8 کاراکتر باشد")]
        public string Password { get; set; }


        [Required(ErrorMessage = "لطفا شماره موبایل کاربر را وارد کنید")]
        [MinLength(11, ErrorMessage = "شماره موبایل کاربر نمی تواند کمتر از 11 کاراکتر باشد")]
        [MaxLength(13, ErrorMessage = "شماره موبایل کاربر نمی تواند از 13 کاراکتر بیشتر باشد")]
        public string PhoneNumber { get; set; }


        [AllowNull]
        [MaxLength(500, ErrorMessage = "آدرس کاربر نمی تواند بیش از 500 کاراتر باشد")]
        public string Address { get; set; }

        [Required(ErrorMessage = "لطفا نوع کاربر را تعیین کنید ")]
        public UserModel.UserRole Role { get; set; }

        [Required(ErrorMessage = "لطفا وضعیت کاربر را تعیین کنید")]
        public bool IsActive { get; set; }
    }
}
