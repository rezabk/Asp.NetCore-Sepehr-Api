using System.ComponentModel.DataAnnotations;


namespace DAL.DTO.Auth
{
    public class LoginDto
    {
        [EmailAddress(ErrorMessage = "لطفا ایمیل را به صورت صحیح وارد کنید")]
        [Required(ErrorMessage = "ایمیل ضروری است")]
        public string Email { get; set; }
        [Required(ErrorMessage = "رمز عبور ضروری است")]
        public string Password { get; set; }
    }
}
