using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using WeatherDisplay.Models.Account;
using WeatherDisplay.Services.Authentication;

namespace WeatherDisplay.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthService _authService;

        //의존성 주입
        public AuthenticationController(
            IAuthService authService
            )
        {
            _authService = authService;
        }

        //Register 페이지를 보여줍니다.
        [HttpGet]
        public IActionResult Register() => View();

        //UserManager 클래스를 통해 새로운 유저를 등록합니다.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if(ModelState.IsValid)
            {
                var result = await _authService.RegisterUserAsync(model);

                if(result.Succeeded)
                {
                    TempData["Message"] = "회원가입에 성공하셨습니다.";
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }


        // GET: Account/Login
        //사용자가 로그인 후 원래있던 페이지로 돌아갈수있게 합니다.
        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login
        //사용자가 로그인 할수있도록 도와주는 클래스입니다.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model, string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.LoginUserAsync(model, returnUrl);

            if (result.Succeeded)
            {
                TempData["Message"] = "성공적으로 로그인하였습니다.";
                return RedirectToLocal(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // POST: Account/Logout
        //유저가 로그아웃할 수 있도록 도와줍니다.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut(string? returnUrl)
        {
            await _authService.LogoutUserAsync();
            TempData["Message"] = "성공적으로 로그아웃하셨습니다.";
            return RedirectToLocal(returnUrl);
        }

        //사용자가 로그아웃 후 원래 있던 페이지로 돌아갈수 있도록 도와주는 클래스입니다.
        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
