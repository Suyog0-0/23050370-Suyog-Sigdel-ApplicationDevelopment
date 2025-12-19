using _23050370_Suyog_Sigdel.Data;
using _23050370_Suyog_Sigdel.Models;

namespace _23050370_Suyog_Sigdel.Services;

public partial class JournalService
{
    // ──────────────────────────────────────────────
    // 🔹 ADD NEW ENTRY (IF NOT EXIST)
    // ──────────────────────────────────────────────
    public async Task AddEntryAsync(string content)
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
                Content = content
            };

            await _db.JournalEntries.AddAsync(entry);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("DB Insert Error: " + ex.Message);
        }
    }
}