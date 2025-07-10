using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WeatherDisplay.Data;
using WeatherDisplay.Models;
using WeatherDisplay.Services.Weather;
using WeatherDisplay.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.Configure<WeatherApiSettings>(
    builder.Configuration.GetSection("WeatherApiSettings"));

builder.Services.AddHttpClient();
builder.Services.AddTransient<IWeatherService, WeatherDisplayService>();


builder.Services.AddAutoMapper(config =>
{
    config.AddMaps(Assembly.GetExecutingAssembly());
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    // 비밀번호 요구사항 설정
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    //이메일을 사용자이름으로 설정
    options.User.RequireUniqueEmail = true;

    // 회원가입 후 이메일 확인 요구사항 설정
    options.SignIn.RequireConfirmedEmail = true; 
})
.AddEntityFrameworkStores<ApplicationDbContext>();

//보안 관련 쿠키기반 설정
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

builder.Services.AddControllersWithViews();


var app = builder.Build();

//HTTP 요청 파이프라인 설정
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
