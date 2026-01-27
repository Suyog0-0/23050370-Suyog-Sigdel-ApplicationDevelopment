using _23050370_Suyog_Sigdel.Data;
using _23050370_Suyog_Sigdel.Models;
using _23050370_Suyog_Sigdel.Helpers;
using Microsoft.EntityFrameworkCore;

namespace _23050370_Suyog_Sigdel.Services;

public partial class JournalService
{
    // ──────────────────────────────────────────────
    // ADD NEW ENTRY (IF NOT EXIST) WITH TAGS AND MOODS
    // ──────────────────────────────────────────────
    public async Task AddEntryAsync(string title, string content, List<string> tagNames, 
        string primaryMood, string? secondaryMood1 = null, string? secondaryMood2 = null)
    {
        try
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var existing = await GetEntryByDateAsync(today);
            if (existing != null)
            {
                Console.WriteLine("Entry already exists. Use UpdateEntryAsync instead.");
                return;
            }

            var entry = new JournalEntryModel
            {
                Date = DateTime.UtcNow,
                EntryDay = today,
                Title = title,
                Content = content,
                PrimaryMood = primaryMood,
                SecondaryMood1 = secondaryMood1,
                SecondaryMood2 = secondaryMood2
            };

            // Add tags if provided
            if (tagNames != null && tagNames.Any())
            {
                foreach (var tagName in tagNames)
                {
                    var tag = await TagHelper.GetOrCreateTagAsync(_db, tagName);
                    entry.Tags.Add(tag);
                }
            }

            await _db.JournalEntries.AddAsync(entry);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("DB Insert Error: " + ex.Message);
        }
    }
}