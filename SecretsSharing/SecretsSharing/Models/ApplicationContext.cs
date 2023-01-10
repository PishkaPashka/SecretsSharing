using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecretsSharing.Models.Secrets;

namespace SecretsSharing.Models
{
    public class ApplicationContext : IdentityDbContext
    {
        public DbSet<TextSecret> TextSecrets { get; set; }
        public DbSet<FileSecret> FileSecrets { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=secrets.db");
        }
    }
}