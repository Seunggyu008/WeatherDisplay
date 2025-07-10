namespace WeatherDisplay.Models.Profile
{
    public class ProfileEditVM
    {
        public ProfileVM Profile { get; set; }
        public FirstNameEditVM FirstNameEdit { get; set; }
        public LastNameEditVM LastNameEdit { get; set; }
        public EmailEditVM EmailEdit { get; set; }
        public DateOfBirthEditVM DateOfBirthEdit { get; set; }
        public PasswordEditVM PasswordEdit { get; set; }
    }
}
