using System;
using System.ComponentModel.DataAnnotations;

namespace _23050370_Suyog_Sigdel.Models;

public class JournalEntry
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;
}