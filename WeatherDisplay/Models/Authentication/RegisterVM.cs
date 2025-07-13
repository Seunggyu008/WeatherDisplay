using System.ComponentModel.DataAnnotations;

namespace WeatherDisplay.Models.Account
{
    public class RegisterVM
    {
        [Required(ErrorMessage ="이메일을 입력해주세요.")]
        [EmailAddress]
        [Display(Name = "이메일")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "성을 입력해주세요.")]
        [StringLength(5, MinimumLength = 1)]
        [Display(Name = "성")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "이름을 입력해주세요.")]
        [StringLength(5, MinimumLength = 1)]
        [Display(Name = "이름")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "생일을 입력해주세요.")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "비밀번호를 입력해주세요.")]
        [StringLength(100, ErrorMessage = "비밀번호는 6자 이상이여야합니다.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "비밀번호")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "비밀번호 입력 후 동일한 비밀번호를 적어주세요.")]
        [DataType(DataType.Password)]
        [Display(Name = "비밀번호 확인")]
        [Compare("Password", ErrorMessage = "입력하신 비밀번호와 달라요.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
