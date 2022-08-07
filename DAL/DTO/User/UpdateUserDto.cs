using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DAL.DTO.User
{
    public class UpdateUserDto
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
        [Required(ErrorMessage = "لطفا تکرار رمز عبور خود را وارد کنید")]
        public string RePassword { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }

        [AllowNull]
        [MaxLength(500, ErrorMessage = "آدرس کاربر نمی تواند بیش از 500 کاراتر باشد")]
        public string Address { get; set; }
    }
}
