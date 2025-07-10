using System.ComponentModel.DataAnnotations;

namespace WeatherDisplay.Models.Profile
{
    public class LastNameEditVM
    {
        [Required(ErrorMessage = "이름을 입력해주세요.")]
        [StringLength(10, MinimumLength = 1)]
        [Display(Name = "이름")]
        public string LastName { get; set; } = "";
    }
}
