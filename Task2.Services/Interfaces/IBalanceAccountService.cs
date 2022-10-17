using Task2.Contracts.Models;

namespace Task2.Services.Interfaces;

public interface IBalanceAccountService
{
    Task FillDatabaseWithExcelAsync(string fileName, MemoryStream stream);
    Task<IEnumerable<BalanceAccount>> GetBalanceAccountsAsync(int excelFileId);
    Task<IEnumerable<ExcelFile>> GetExcelFilesAsync();
}