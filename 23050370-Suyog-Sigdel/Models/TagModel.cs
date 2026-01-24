using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _23050370_Suyog_Sigdel.Models;

[Table("tags")]
public class TagModel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    // Navigation property for many-to-many relationship
    public List<JournalEntryModel> JournalEntries { get; set; } = new();
}