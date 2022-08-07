using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;


namespace DAL.DTO.Auth
{
    public class RegisterDto
    {
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
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا تکرار رمز عبور خود را وارد کنید")]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "لطفا ایمیل خود را وارد کنید")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده صحیح نیست")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا شماره موبایل خود را وارد کنید")]
        [MinLength(11, ErrorMessage = "شماره موبایل نمی تواند کمتر از 11 کاراکتر باشد")]
        [MaxLength(13, ErrorMessage = "شماره موبایل نمی تواند از 13 کاراکتر بیشتر باشد")]
        public string PhoneNumber { get; set; }

        [AllowNull]
        [MaxLength(500, ErrorMessage = "آدرس نمی تواند بیش از 500 کاراتر باشد")]
        public string Address { get; set; }
    }
}
