using System.ComponentModel.DataAnnotations;

namespace WeatherDisplay.Models.Profile
{
    public class PasswordEditVM
    {    
        [DataType(DataType.Password)]
        [Display(Name = "현재 비밀번호")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "새 비밀번호")]
        [MinLength(6, ErrorMessage = "비밀번호는 최소 6자리 이상이어야 합니다.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "비밀번호 확인")]
        [Compare("NewPassword", ErrorMessage = "비밀번호가 일치하지 않습니다.")]
        public string ConfirmPassword { get; set; }
    }
}
