using System.ComponentModel.DataAnnotations;

namespace WeatherDisplay.Models.Profile
{
    public class EmailEditVM
    {
        [Required(ErrorMessage = "이메일을 입력해주세요.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
