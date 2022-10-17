using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1.Sql.Entities;

[Table("Task1")]
public class Task1Entity
{
    [Key] public int Id { get; set; }
    public DateTime RandomDate { get; set; }
    public string RandomLatinString { get; set; }
    public string RandomCyrillicString { get; set; }
    public int RandomInteger { get; set; }
    public float RandomFloat { get; set; }
}