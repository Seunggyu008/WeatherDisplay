using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WeatherDisplay.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            /*
            var hasher = new PasswordHasher<ApplicationUser>();

            //ModelBuilder를 사용하여 디폴트 데이터를 시딩 해놓습니다.
            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "56c9ae30-eac6-4275-956d-d475dc09c5d5",
                    Email = "admin@localhost.com",
                    NormalizedEmail = "ADMIN@LOCALHOST.COM",
                    NormalizedUserName = "ADMIN@LOCALHOST.COM",
                    UserName = "admin@localhost.com",
                    //because password is hashed, we need to initialize a hasher
                    PasswordHash = hasher.HashPassword(null, "P@assword1"),
                    EmailConfirmed = true,
                    FirstName = "Default",
                    LastName = "Admin",
                    DateOfBirth = new DateOnly(1950, 12, 01)
                }
            );
            */

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    
    }
}
