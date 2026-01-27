using _23050370_Suyog_Sigdel.Data;
using _23050370_Suyog_Sigdel.Helpers;
using _23050370_Suyog_Sigdel.Models;
using Microsoft.EntityFrameworkCore;

namespace _23050370_Suyog_Sigdel.Services;

public partial class JournalService
{
    
    
    public async Task UpdateEntryByIdAsync(
        int id,
        string title,
        string content,
        List<string> tagNames,
        string primaryMood,
        string? secondaryMood1 = null,
        string? secondaryMood2 = null)
    {
        var entry = await _db.JournalEntries
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (entry == null)
            return; 

        entry.Title = title;
        entry.Content = content;
        entry.PrimaryMood = primaryMood;
        entry.SecondaryMood1 = secondaryMood1;
        entry.SecondaryMood2 = secondaryMood2;
        entry.Date = DateTime.UtcNow; // Tracks last modification time

        // Replace tags with new set
        entry.Tags.Clear();
    
        foreach (var name in tagNames.Distinct())
        {
            var normalized = name.Trim().ToLower();
            var tag = await _db.Tags.FirstOrDefaultAsync(t => t.Name == normalized) 
                      ?? new TagModel { Name = normalized };
        
            if (tag.Id == 0) _db.Tags.Add(tag);
            entry.Tags.Add(tag);
        }

        await _db.SaveChangesAsync();
    }


    
    
    
    
    
}