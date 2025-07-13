using Microsoft.AspNetCore.Mvc;
using System;
using WeatherDisplay.Models.Account;

namespace WeatherDisplay.Services.Authentication
{
    public interface IAuthService
    {
        //반환하고 싶은 값을 Task 이후에 넣는다.
        Task<IdentityResult>RegisterUserAsync(RegisterVM model);
        Task<IdentityResult> LoginUserAsync(LoginVM model, string? returnUrl);
        Task LogoutUserAsync();
    }
    public class AppAuthenticationService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ApplicationUser> _logger;

        public AppAuthenticationService (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<ApplicationUser> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        //회원가입 로직을 당담하는 서비스입니다.
        public async Task<IdentityResult> RegisterUserAsync(RegisterVM model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return IdentityResult.Failed(new IdentityError { Description = "계정 생성에 실패하였습니다." });
            }
            else
            {
                _logger.LogInformation("새로운 계정이 성공적으로 생성되었습니다.");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return result;
            }
        }

        //로그인 로직을 당담하는 서비스입니다.
        public async Task<IdentityResult> LoginUserAsync(LoginVM model, string? returnUrl)
        {
            var result = await _signInManager.PasswordSignInAsync(
                userName: model.Email,
                password: model.Password,
                isPersistent: model.RememberMe,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                _logger.LogInformation("성공적으로 로그인하였습니다");
                return (IdentityResult.Success);
            }

            var errors = new List<string>();
            
            if(result.IsLockedOut)
            {
                _logger.LogWarning("사용자 계정이 잠겼습니다.");
                errors.Add("계정이 잠겼습니다");
            } else if(result.IsNotAllowed)
            {
                _logger.LogWarning("로그인 불가, 이메일 인증 여부 확인");
                errors.Add("로그인이 허용되지 않았습니다. 이메일 인증을 완료했는지 확인해주세요.");
            } else
            {
                _logger.LogWarning("아이디 또는 비밀번호가 틀렸습니다");
                errors.Add("아이디 또는 비밀번호가 틀렸습니다.");
            }

            return IdentityResult.Failed(errors.Select(e => new IdentityError { Description = e }).ToArray()
         );
        }

        //로그아웃 로직을 당담하는 서비스입니다.
        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User Logged Out");
        }      
    }
}
