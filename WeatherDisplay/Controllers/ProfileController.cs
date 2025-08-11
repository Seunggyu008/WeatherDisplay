using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WeatherDisplay.Models.Profile;
//using AutoMapper;
using WeatherDisplay.Services.Profile;

namespace WeatherDisplay.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IMapper _mapper;
        
        public ProfileController(IProfileService profileService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _profileService = profileService;
            _userManager = userManager;
            _signInManager = signInManager;
            //_mapper = mapper;
        }

        [HttpGet]
        //사용자 정보를 가져오기위한 메서드입니다.
        public async Task<IActionResult> GetUserProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var viewModel = new ProfileEditAllVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                PasswordEdit = new PasswordEditVM()
            };
       
            /*
            var viewModel = new ProfileEditVM
            {
                Profile = profileModel,
                FirstNameEdit = new FirstNameEditVM { FirstName = profileModel.FirstName },
                LastNameEdit = new LastNameEditVM { LastName = profileModel.LastName },
                EmailEdit = new EmailEditVM { Email = profileModel.Email },
                DateOfBirthEdit = new DateOfBirthEditVM { DateOfBirth = profileModel.DateOfBirth },
                PasswordEdit = new PasswordEditVM()
            };
            */

            return View("UserProfileAll", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserProfile(ProfileEditAllVM model)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _profileService.EditAllProfileAsync(userId, model);
            if (result.Succeeded)
            {
                TempData["StatusMessage"] = "프로필이 성공적으로 변경되었습니다.";
                return RedirectToAction("GetUserProfile");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("UserProfile", model);
        }

            //사용자 성을 변경하기 위한 메서드입니다.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserFirstName(FirstNameEditVM model)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _profileService.EditFirstNameAsync(userId, model);

            if (result.Succeeded)
            {
                TempData["StatusMessage"] = "성이 성공적으로 변경되었습니다.";
                return RedirectToAction("GetUserProfile");
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("FirstName", "이미 사용중이던 성입니다.");
            }

            return View("UserProfile", model);           
        }

        //사용자 이름 변경을 위한 메서드입니다.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserLastName(LastNameEditVM model)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _profileService.EditLastNameAsync(userId, model);

            if (result.Succeeded)
            {
                TempData["StatusMessage"] = "이름이 성공적으로 변경되었습니다.";
                return RedirectToAction("GetUserProfile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("LastName", "이미 사용중이던 이름입니다.");
            }

            return View("UserProfile", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //생년월일 변경을 위한 메서드입니다.
        public async Task<IActionResult> EditUserBirthDate(DateOfBirthEditVM model)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _profileService.EditDobAsync(userId, model);

            if (result.Succeeded)
            {
                TempData["StatusMessage"] = "생년월일이 성공적으로 변경되었습니다.";
                return RedirectToAction("GetUserProfile", model);
            }   

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("DateOfBirth", "이미 사용중이던 생년월일입니다.");
            }

            return View("UserProfile", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //이메일 변경을 위한 메서드입니다.
        public async Task<IActionResult> EditUserEmail(EmailEditVM model)
        {
            var userId = _userManager.GetUserId(User);

            var (result, user) = await _profileService.EditEmailAsync(userId, model);

            if(result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["StatusMessage"] = "이메일이 성공적으로 변경되었습니다.";
                return RedirectToAction("GetUserProfile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Email", "이미 사용중이던 이메일입니다.");
            }
      
            return View("UserProfile", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //비밀번호 변경을 위한 메서드입니다.
        public async Task<IActionResult> ChangeUserPassword(PasswordEditVM model)
        {

            var userId = _userManager.GetUserId(User);
            var (result, user) = await _profileService.ChangePasswordAsync(userId, model);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["StatusMessage"] = "비밀번호가 성공적으로 변경되었습니다";
                return RedirectToAction("GetUserProfile");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("Password", "비밀번호 변경이 실패하였습니다.");

            return View("UserProfile", model);
        }
    }
}
