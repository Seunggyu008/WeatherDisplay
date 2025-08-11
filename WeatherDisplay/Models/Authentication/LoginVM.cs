using System.ComponentModel.DataAnnotations;

namespace WeatherDisplay.Models.Account
{
    public class LoginVM 
    {
        [Required]
        [EmailAddress]
        [Display(Name = "이메일")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "비밀번호")]
        public string Password { get; set; } 

        [Display(Name = "자동 로그인")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
