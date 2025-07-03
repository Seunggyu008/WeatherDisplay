using Microsoft.AspNetCore.Identity;
using WeatherDisplay.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WeatherDisplay.Models.Authentication;

namespace WeatherDisplay.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //Inject Denpendency
        public ProfileController(UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
        }

        //fetch profile
        [HttpGet]
        public async Task<IActionResult> GetUserProfile()
        {       
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return NotFound();
            }

            var model = new ProfileVM
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth
            };


            return View("UserProfile", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUserProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var model = new ProfileEditVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth
            };

            return View("UserProfileEdit", model); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserProfile(ProfileEditVM model)
        {
            if (!ModelState.IsValid)
            {
                // 유효성 검사 실패 시 -> 뷰 다시 보여줌
                return View("~/Views/Profile/UserProfileEdit.cshtml", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.DateOfBirth = model.DateOfBirth;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["StatusMessage"] = "프로필이 성공적으로 수정되었습니다.";
                return RedirectToAction("GetUserProfile");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View("UserProfileEdit", model);
        }
    }
}
