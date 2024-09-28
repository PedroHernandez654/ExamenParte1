using ExamenParte1.Models.Entity;
using Microsoft.EntityFrameworkCore;


namespace ExamenParte1.DataBase
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Articulos>? Articulos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LXAZAR-PHERNAND\\MSSQLSERVEREXPRE;Database=ExamenDB;Trusted_Connection=SSPI;MultipleActiveResultSets=true;TrustServerCertificate=True");
        }
    }
}
