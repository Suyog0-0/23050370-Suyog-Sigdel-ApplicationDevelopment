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

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("date")]
    public DateTime Date { get; set; } = DateTime.UtcNow;
    
    [Column("entry_day")]
    public DateOnly EntryDay { get; set; }
    
    // Mood tracking properties
    [Column("primary_mood")]
    public string PrimaryMood { get; set; } = string.Empty;  // Required
    
    [Column("secondary_mood_1")]
    public string? SecondaryMood1 { get; set; }  // Optional
    
    [Column("secondary_mood_2")]
    public string? SecondaryMood2 { get; set; }  // Optional
    
    // Navigation property - many-to-many relationship with tags
    public List<TagModel> Tags { get; set; } = new();
}