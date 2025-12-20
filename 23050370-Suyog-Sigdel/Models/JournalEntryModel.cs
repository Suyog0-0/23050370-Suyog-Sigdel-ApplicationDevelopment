using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _23050370_Suyog_Sigdel.Models;

[Table("journal_entries")]
public class JournalEntryModel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("date")]
    public DateTime Date { get; set; } = DateTime.UtcNow;
    
    [Column("entry_day")]
    public DateOnly EntryDay { get; set; } 
}


