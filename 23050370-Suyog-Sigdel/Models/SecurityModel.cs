using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _23050370_Suyog_Sigdel.Models;

[Table("security")]
public class SecurityModel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("pin")]
    public string Pin { get; set; } = string.Empty;

    [Column("is_enabled")]
    public bool IsEnabled { get; set; } = false;
}