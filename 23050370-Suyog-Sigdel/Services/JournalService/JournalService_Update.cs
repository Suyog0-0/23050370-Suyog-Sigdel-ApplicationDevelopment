using _23050370_Suyog_Sigdel.Data;
using _23050370_Suyog_Sigdel.Models;

namespace _23050370_Suyog_Sigdel.Services;

public partial class JournalService
{
    // ──────────────────────────────────────────────
    // 🔹 UPDATE ENTRY FOR TODAY
    // ──────────────────────────────────────────────
    public async Task UpdateEntryAsync(string content)
    {
        try
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var existing = await GetEntryByDateAsync(today);

            if (existing == null)
            {
                Console.WriteLine("No entry found for today. Use AddEntryAsync first.");
                return;
            }

            existing.Content = content;
            existing.Date = DateTime.UtcNow;

            _db.JournalEntries.Update(existing);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("DB Update Error: " + ex.Message);
        }
    }
}