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
            optionsBuilder.UseMySQL(configuration.GetConnectionString("AuthDB") ?? throw new Exception("Impossible to connect to the db"));
        }

        public DbSet<Address> Address { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //sovrascrivo il comportamento di identity. Questo per evitare conflitti e tabella duplicate con tipi diversi
            // N.B il tipo di deafault per la chiave è string, quindi qual'ora si voglia cambiare il tipo bisogna ignorare le tabelle identity come chiave stringa nella migrazione
            modelBuilder.HasDefaultSchema("auth");

            // Change table names
            modelBuilder.Entity<IdentityUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            // We'll create a relation between userId of IdentityUser and userId of Address 1-1
            modelBuilder.Entity<Address>().HasOne(x => x.User).WithOne().HasForeignKey<Address>(x => x.UserId).IsRequired();

        }

    }
}
