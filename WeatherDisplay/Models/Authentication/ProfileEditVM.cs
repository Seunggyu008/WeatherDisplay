using System.ComponentModel.DataAnnotations;

namespace WeatherDisplay.Models.Authentication
{
    public class ProfileEditVM 
    {
        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
    }
}
