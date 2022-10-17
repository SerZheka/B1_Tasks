namespace Task2.Contracts.Models;

public class ExcelFile
{
    public int FileId { get; set; }
    public string FileName { get; set; }
    
    public IList<BalanceAccount> BalanceAccounts { get; set; }
}