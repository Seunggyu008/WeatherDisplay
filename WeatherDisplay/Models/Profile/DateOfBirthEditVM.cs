using System.ComponentModel.DataAnnotations;

namespace WeatherDisplay.Models.Profile
{
    public class DateOfBirthEditVM
    {
        [Required(ErrorMessage = "생일을 입력해주세요.")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

    }
}
