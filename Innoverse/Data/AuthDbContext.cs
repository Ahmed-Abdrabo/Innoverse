using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Innoverse.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Suppress PendingModelChangesWarning
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "6b11f638-7a8e-4716-b2dd-b12cb11f0a4c";
            var writerRoleId = "04b6203b-c07c-49da-bb0c-f5022b772772";
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name ="Reader",
                    NormalizedName ="Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name ="Writer",
                    NormalizedName ="Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            var adminUserId = "2be59d1d-3ff5-4b46-833c-31bdcebd0f45";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@innoverse.com",
                Email = "admin@innoverse.com",
                NormalizedEmail = "admin@innoverse.com".ToUpper(),
                NormalizedUserName = "admin@innoverse.com".ToUpper(),
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");
            builder.Entity<IdentityUser>().HasData(admin);

            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId =readerRoleId
                },

                new()
                {
                    UserId = adminUserId,
                    RoleId =writerRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

        }
    }
}
