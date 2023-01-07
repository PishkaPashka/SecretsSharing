using Microsoft.EntityFrameworkCore;

namespace SecretsSharing
{
    public class ApplicationContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=secrets.db");
        }
    }
}