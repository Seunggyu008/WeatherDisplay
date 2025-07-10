using Microsoft.AspNetCore.Identity;
using WeatherDisplay.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WeatherDisplay.Models.Profile;
using AutoMapper;

namespace WeatherDisplay.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        
        public ProfileController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpGet]
        //사용자 정보를 가져오기위한 메서드입니다.
        public async Task<IActionResult> GetUserProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var profileModel = _mapper.Map<ProfileVM> (user);

            var viewModel = new ProfileEditVM
            {
                Profile = profileModel,
                FirstNameEdit = new FirstNameEditVM { FirstName = profileModel.FirstName },
                LastNameEdit = new LastNameEditVM { LastName = profileModel.LastName },
                EmailEdit = new EmailEditVM { Email = profileModel.Email },
                DateOfBirthEdit = new DateOfBirthEditVM { DateOfBirth = profileModel.DateOfBirth },
                PasswordEdit = new PasswordEditVM()
            };

            return View("UserProfile", viewModel);
        }

        //사용자 성을 변경하기 위한 메서드입니다.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserFirstName(FirstNameEditVM model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var profileModel = _mapper.Map<ProfileVM>(user);

            if (!ModelState.IsValid)
            {
                var viewModel = new ProfileEditVM
                {
                    Profile = profileModel,
                    FirstNameEdit = model,
                    LastNameEdit = new LastNameEditVM { LastName = profileModel.LastName },
                    EmailEdit = new EmailEditVM { Email = profileModel.Email },
                    DateOfBirthEdit = new DateOfBirthEditVM { DateOfBirth = profileModel.DateOfBirth },
                    PasswordEdit = new PasswordEditVM()
                };
                return View("UserProfile", viewModel);
            }

            user.FirstName = model.FirstName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["StatusMessage"] = "성이 성공적으로 변경되었습니다.";
                return RedirectToAction("GetUserProfile");
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("FirstName", "이미 사용중이던 성입니다.");
            }

            return View("UserProfile", profileModel);           
        }

        //사용자 이름 변경을 위한 메서드입니다.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserLastName(LastNameEditVM model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var profileModel = _mapper.Map<ProfileVM>(user);

            if (!ModelState.IsValid)
            {
                var viewModel = new ProfileEditVM
                {
                    Profile = profileModel,
                    FirstNameEdit = new FirstNameEditVM { FirstName = profileModel.FirstName },
                    LastNameEdit = model,
                    EmailEdit = new EmailEditVM { Email = profileModel.Email },
                    DateOfBirthEdit = new DateOfBirthEditVM { DateOfBirth = profileModel.DateOfBirth },
                    PasswordEdit = new PasswordEditVM()
                };
                return View("UserProfile", viewModel);
            }

            user.LastName = model.LastName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["StatusMessage"] = "이름이 성공적으로 변경되었습니다.";
                return RedirectToAction("GetUserProfile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("LastName", "이미 사용중이던 이름입니다.");
            }

            return View("UserProfile", profileModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //생년월일 변경을 위한 메서드입니다.
        public async Task<IActionResult> EditUserBirthDate(DateOfBirthEditVM model)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var profileModel = _mapper.Map<ProfileVM>(user);

            if (!ModelState.IsValid)
            {
                var viewModel = new ProfileEditVM
                {
                    Profile = profileModel,
                    FirstNameEdit = new FirstNameEditVM { FirstName = profileModel.FirstName },
                    LastNameEdit = new LastNameEditVM { LastName = profileModel.LastName },
                    EmailEdit = new EmailEditVM { Email = profileModel.Email },
                    DateOfBirthEdit = model,
                    PasswordEdit = new PasswordEditVM()
                };
                return View("UserProfile", viewModel);
            }

            user.DateOfBirth = model.DateOfBirth;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["StatusMessage"] = "생년월일이 성공적으로 변경되었습니다.";
                return RedirectToAction("GetUserProfile", model);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("DateOfBirth", "이미 사용중이던 생년월일입니다.");
            }

            return View("UserProfile", profileModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //이메일 변경을 위한 메서드입니다.
        public async Task<IActionResult> EditUserEmail(EmailEditVM model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return RedirectToAction("Login");

            //var currentEmail = await _userManager.GetEmailAsync(user);
            var profileModel = _mapper.Map<ProfileVM>(user);

            if (model.Email == user.Email)
            {
                ModelState.AddModelError("Email", "기존 이메일과 동일합니다");         
            }

            if (!ModelState.IsValid)
            {
                var viewModel = new ProfileEditVM
                {
                    Profile = profileModel,
                    FirstNameEdit = new FirstNameEditVM { FirstName = profileModel.FirstName },
                    LastNameEdit = new LastNameEditVM { LastName = profileModel.LastName },
                    EmailEdit = model,
                    DateOfBirthEdit = new DateOfBirthEditVM { DateOfBirth = profileModel.DateOfBirth },
                    PasswordEdit = new PasswordEditVM()
                };
                return View("UserProfile", viewModel);
            }

            var emailResult = await _userManager.SetEmailAsync(user, model.Email);
            var usernameResult = await _userManager.SetUserNameAsync(user, model.Email);

            if (emailResult.Succeeded && usernameResult.Succeeded)
            {
                TempData["StatusMessage"] = "이메일이 성공적으로 변경되었습니다.";
                return RedirectToAction("GetUserProfile");
            }

            foreach (var error in emailResult.Errors)
            {
                ModelState.AddModelError("Email", "이미 사용중이던 이메일입니다.");
            }
      
            return View("UserProfile", profileModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserPassword(PasswordEditVM model)
        {
          
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var profileModel = _mapper.Map<ProfileVM>(user);         

            if (!ModelState.IsValid)
            {
                var viewModel = new ProfileEditVM
                {
                    Profile = profileModel,
                    FirstNameEdit = new FirstNameEditVM { FirstName = user.FirstName },
                    LastNameEdit = new LastNameEditVM { LastName = user.LastName },
                    EmailEdit = new EmailEditVM { Email = user.Email },
                    DateOfBirthEdit = new DateOfBirthEditVM { DateOfBirth = user.DateOfBirth },
                    PasswordEdit = model 
                };
                return View("UserProfile", viewModel);
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["StatusMessage"] = "비밀번호가 성공적으로 변경되었습니다";
                return RedirectToAction("GetUserProfile");
            }           

            foreach (var error in result.Errors)
                ModelState.AddModelError("Password", "비밀번호 변경이 실패하였습니다.");

            return View("UserProfile", profileModel);
        }
    }
}
