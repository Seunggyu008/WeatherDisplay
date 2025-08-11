using AutoMapper;
using Humanizer;
using System.Security.Claims;
using WeatherDisplay.Models.Profile;
using WeatherDisplay.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WeatherDisplay.Services.Profile
{
    public interface IProfileService
    {

        //Task<ServiceResult<ProfileVM>> GetUserProfileAsync(string userId);
        Task<IdentityResult> EditAllProfileAsync(string? userId, ProfileEditAllVM model);
        Task<IdentityResult> EditFirstNameAsync(string userId, FirstNameEditVM model);
        Task<IdentityResult> EditLastNameAsync(string userId, LastNameEditVM model);
        Task<IdentityResult> EditDobAsync(string userId, DateOfBirthEditVM model);
        Task<(IdentityResult result, ApplicationUser user)> EditEmailAsync(string userId, EmailEditVM model);

        Task<(IdentityResult result, ApplicationUser user)> ChangePasswordAsync(string userId, PasswordEditVM model);

    }
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ProfileService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> EditAllProfileAsync(string? userId, ProfileEditAllVM model)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "사용자를 찾을 수 없습니다." });
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.DateOfBirth = model.DateOfBirth;
            user.Email = model.Email;

            return await _userManager.UpdateAsync(user);
        }

        //성을 바꾸는 로직을 당담하는 서비스입니다.
        public async Task<IdentityResult> EditFirstNameAsync (string? userId, FirstNameEditVM model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "사용자를 찾을 수 없습니다." });
            }

            user.FirstName = model.FirstName;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> EditLastNameAsync(string userId, LastNameEditVM model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "사용자를 찾을 수 없습니다." });
            }

            user.LastName = model.LastName;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> EditDobAsync(string userId, DateOfBirthEditVM model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "사용자를 찾을 수 없습니다." });
            }

            user.DateOfBirth = model.DateOfBirth;
            return await _userManager.UpdateAsync(user);

        }

        public async Task<(IdentityResult result, ApplicationUser user)> EditEmailAsync(string userId, EmailEditVM model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (IdentityResult.Failed(new IdentityError { Description = "사용자를 찾을 수 없습니다." }), null);
            }

            if(string.Equals(user.Email, model.Email, StringComparison.OrdinalIgnoreCase))
            {
                var sameEmailError = new IdentityError
                {
                    Code = "SameEmail",
                    Description = "새 이메일 주소가 현재 이메일 주소와 동일합니다."
                };
                return (IdentityResult.Failed(sameEmailError), null);
            }

            //이메일 설정 검증
            var emailResult = await _userManager.SetEmailAsync(user, model.Email);
            if(!emailResult.Succeeded)
            {
                return (emailResult, null);
            }

            //사용자 이름 설정 검증
            var usernameResult = await _userManager.SetUserNameAsync(user, model.Email);
            if(!usernameResult.Succeeded)
            {          
                return (usernameResult, null);
            }      
            return (IdentityResult.Success, user);
        }

        public async Task<(IdentityResult result, ApplicationUser user)> ChangePasswordAsync(string userId, PasswordEditVM model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (IdentityResult.Failed(new IdentityError { Description = "사용자를 찾을 수 없습니다." }), null);
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            return (result, user);   
        }
    }
}
