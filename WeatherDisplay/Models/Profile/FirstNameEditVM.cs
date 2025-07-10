using System.ComponentModel.DataAnnotations;


namespace WeatherDisplay.Models.Profile
{
    public class FirstNameEditVM
    {
        [Required(ErrorMessage = "성을 입력해주세요.")]
        [StringLength(10, MinimumLength = 1)]
        [Display(Name = "성")]
        public string FirstName { get; set; } = "";
    }
}
