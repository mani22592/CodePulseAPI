using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //The below 2 Guid generated from C# Interactive window for testing purpose;
            //View -> Other Windows -> C# Interactive then type Guid.NewGuid()
            var readerRole = "5fbe84d0-93b8-4b75-b189-bdaeed27385f";
            var writerRole = "ba891c5b-fadb-492c-b8d2-3b8b3729586a";

            //create reader and writer roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRole,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRole
                },
                new IdentityRole
                {
                    Id = writerRole,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRole
                }
            };

            //seed roles to AspNetRoles table
            builder.Entity<IdentityRole>().HasData(roles);

            //Create an admin user
            var adminUserId = "39af2c16-1279-4ec0-a9ab-400e45014543";
            var admin = new IdentityUser
            {
                Id = adminUserId,
                UserName = "admin@codepulse.com",
                Email = "admin@codepulse.com",
                NormalizedUserName = "admin@codepulse.com".ToUpper(),
                NormalizedEmail = "admin@codepulse.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            builder.Entity<IdentityUser>().HasData(admin);

            //Assign admin user to Writer and Reader roles
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = readerRole,
                    UserId = adminUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId = writerRole,
                    UserId = adminUserId
                }
            );

        }
    }
}
