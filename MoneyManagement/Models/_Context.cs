using Microsoft.EntityFrameworkCore;

namespace MoneyManagement.Models
{
    public class _Context : DbContext
    {
        public DbSet<Cards> CardTbl { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=.;database=MoneyManageMent;trusted_Connection=true;trustservercertificate=true;");
        }
    }
}
