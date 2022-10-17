using Microsoft.EntityFrameworkCore;
using Task1.Entities;

namespace Task1;

public class Task1Context : DbContext
{
    public DbSet<Task1Entity> Task1 { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=b1;User Id=sa;Password=Pa$$w0rd!;");
    }
}