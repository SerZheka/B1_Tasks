using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task2.DataAccess.Sql.Entities;

[Table("ExcelFiles")]
public class ExcelFileEntity
{
    [Key]
    public int FileId { get; set; }
    public string FileName { get; set; }
    
    public virtual IList<BalanceAccountEntity> BalanceAccounts { get; set; }
}