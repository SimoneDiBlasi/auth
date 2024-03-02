using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace auth.Handlers.Model
{
    public class AuthDbContext : IdentityDbContext
    {
        public readonly IConfiguration configuration;
        public AuthDbContext(DbContextOptions<AuthDbContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.Write(configuration.GetConnectionString("AuthDB"));
            optionsBuilder.UseMySQL("server=localhost;port=3306;database=auth;user=root;password=12345678");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("dbo");

            // Change table names
            modelBuilder.Entity<IdentityUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles").HasKey(val => new { val.RoleId, val.UserId });
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins").HasKey(val => val.UserId);
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens").HasKey(val => val.UserId);
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims").HasKey(val => val.RoleId);


        }

    }
}
