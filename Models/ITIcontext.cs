using Microsoft.EntityFrameworkCore;

namespace Hospitel_Project.Models
{
    public class ITIContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-JC03CE9;Initial Catalog=HospitalDataBase;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
            }
        }
        
    }
}
