using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task2.DataAccess.Sql.Entities;

[Table("BalanceAccounts")]
public class BalanceAccountEntity
{
    [Key]
    public int Id { get; set; }
    public int BalanceAccountId { get; set; }
    public double IncomingBalanceAsset { get; set; }
    public double IncomingBalanceLiability { get; set; }
    public double TurnoverAsset { get; set; }
    public double TurnoverLiability { get; set; }
    public double OutgoingBalanceAsset { get; set; }
    public double OutgoingBalanceLiability { get; set; }
    public int ExcelFileId { get; set; }
    public virtual ExcelFileEntity ExcelFile { get; set; }
}