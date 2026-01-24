using _23050370_Suyog_Sigdel.Data;
using Microsoft.EntityFrameworkCore;

namespace _23050370_Suyog_Sigdel.Services;

public partial class JournalService
{
    // ──────────────────────────────────────────────
    // UPDATE ENTRY BY ID WITH TAGS
    // ──────────────────────────────────────────────
    public async Task UpdateEntryByIdAsync(int id, string title, string content, List<string> tagNames)
    {
        try
        {
            var entry = await _db.JournalEntries
                .Include(e => e.Tags)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entry != null)
            {
                entry.Title = title;
                entry.Content = content;
                entry.Date = DateTime.UtcNow;

                // Update tags
                entry.Tags.Clear();
                if (tagNames != null && tagNames.Any())
                {
                    foreach (var tagName in tagNames)
                    {
                        var tag = await GetOrCreateTag(tagName);
                        entry.Tags.Add(tag);
                    }
                }

                _db.JournalEntries.Update(entry);
                await _db.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("DB Update Error: " + ex.Message);
        }
    }
}