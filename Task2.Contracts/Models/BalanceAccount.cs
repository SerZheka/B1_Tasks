namespace Task2.Contracts.Models;

public class BalanceAccount
{
    public int Id { get; set; }
    public int BalanceAccountId { get; set; }
    public double IncomingBalanceAsset { get; set; }
    public double IncomingBalanceLiability { get; set; }
    public double TurnoverAsset { get; set; }
    public double TurnoverLiability { get; set; }
    public double OutgoingBalanceAsset { get; set; }
    public double OutgoingBalanceLiability { get; set; }
    public ExcelFile ExcelFile { get; set; }
}