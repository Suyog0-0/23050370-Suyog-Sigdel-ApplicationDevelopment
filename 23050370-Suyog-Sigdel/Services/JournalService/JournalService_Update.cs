using _23050370_Suyog_Sigdel.Data;
using _23050370_Suyog_Sigdel.Models;
using _23050370_Suyog_Sigdel.Helpers;
using Microsoft.EntityFrameworkCore;

namespace _23050370_Suyog_Sigdel.Services;

public partial class JournalService
{
    // ──────────────────────────────────────────────
    // UPDATE ENTRY FOR TODAY WITH TAGS AND MOODS
    // ──────────────────────────────────────────────
    public async Task UpdateEntryAsync(string title, string content, List<string> tagNames,
        string primaryMood, string? secondaryMood1 = null, string? secondaryMood2 = null)
    {
        try
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var existing = await _db.JournalEntries
                .Include(e => e.Tags)
                .FirstOrDefaultAsync(e => e.EntryDay == today);

            if (existing == null)
            {
                Console.WriteLine("No entry found for today. Use AddEntryAsync first.");
                return;
            }

            existing.Title = title;
            existing.Content = content;
            existing.Date = DateTime.UtcNow;
            existing.PrimaryMood = primaryMood;
            existing.SecondaryMood1 = secondaryMood1;
            existing.SecondaryMood2 = secondaryMood2;

            // Update tags
            existing.Tags.Clear();
            if (tagNames != null && tagNames.Any())
            {
                foreach (var tagName in tagNames)
                {
                    var tag = await TagHelper.GetOrCreateTagAsync(_db, tagName);
                    existing.Tags.Add(tag);
                }
            }

            _db.JournalEntries.Update(existing);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("DB Update Error: " + ex.Message);
        }
    }
}