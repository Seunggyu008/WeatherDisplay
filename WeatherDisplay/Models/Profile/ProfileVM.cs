using System.ComponentModel.DataAnnotations;

namespace WeatherDisplay.Models.Profile
{
    public class ProfileVM 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
