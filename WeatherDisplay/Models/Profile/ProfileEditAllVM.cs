using System.ComponentModel.DataAnnotations;

namespace WeatherDisplay.Models.Profile
{
    public class ProfileEditAllVM
    {
        [Required(ErrorMessage = "성을 입력해주세요.")]
        [StringLength(10, MinimumLength = 1)]
        [Display(Name = "성")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "이름을 입력해주세요.")]
        [StringLength(10, MinimumLength = 1)]
        [Display(Name = "이름")]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "생일을 입력해주세요.")]
        [DataType(DataType.Date)]
        [Display(Name = "생년월일")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "이메일을 입력해주세요.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public PasswordEditVM PasswordEdit { get; set; }
    }
}
