using Microsoft.AspNetCore.Identity;
using WeatherDisplay.Data;
using Microsoft.AspNetCore.Mvc;
using WeatherDisplay.Models.Account;
using System.Threading.Tasks;


namespace WeatherDisplay.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ApplicationUser> _logger;

        //의존성 주입
        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<ApplicationUser> logger
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // GET: Account/Register
        //Register 페이지를 보여줍니다.
        [HttpGet]
        public IActionResult Register() => View();

        // POST: Account/Register
        //UserManager 클래스를 통해 새로운 유저를 등록합니다.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,

                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors) {
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
        //사용자가 로그인 할수있도록 하고,
        //RedirectToLocal 메서드 사용하여 로그인 후 원래 있던 페이지로 돌아갈 수 있도록 합니다.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model, string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                userName: model.Email,
                password: model.Password,
                isPersistent: model.RememberMe,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }

            if(result.IsLockedOut)
            {
                ModelState.AddModelError("", "계정이 잠겼어요.");
            } else if(result.IsNotAllowed)
            {
                ModelState.AddModelError("", "이메일 인증 완료가 안됬어요,");
            } else if(result.RequiresTwoFactor)
            {
                ModelState.AddModelError("", "2단계 인증이 필요해요.");
            } else
            {
                ModelState.AddModelError("", "이메일 또는 비밀번호가 틀렸어요.");
            }

            return View(model);
        }

        // POST: Account/Logout
        //유저가 로그아웃할 수 있도록 도와줍니다.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut(string? returnUrl)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User Logged Out");
            return RedirectToLocal(returnUrl);
        }

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
