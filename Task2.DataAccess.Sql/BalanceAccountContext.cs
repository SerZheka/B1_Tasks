using Microsoft.EntityFrameworkCore;
using Task2.DataAccess.Sql.Entities;

namespace Task2.DataAccess.Sql;

public sealed class BalanceAccountContext : DbContext
{
    public BalanceAccountContext(DbContextOptions<BalanceAccountContext> contextOptionsBuilder)
        : base(contextOptionsBuilder)
    {
        Database.EnsureCreated();
    }

    public DbSet<BalanceAccountEntity> BalanceAccounts { get; set; }
    public DbSet<ExcelFileEntity> ExcelFiles { get; set; }
}