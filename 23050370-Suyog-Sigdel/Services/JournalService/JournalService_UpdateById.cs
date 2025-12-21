using _23050370_Suyog_Sigdel.Data;

namespace _23050370_Suyog_Sigdel.Services;

public partial class JournalService
{
    // ──────────────────────────────────────────────
    // UPDATE ENTRY BY ID
    // ──────────────────────────────────────────────
    public async Task UpdateEntryByIdAsync(int id, string title, string content)
    {
        try
        {
            var entry = await _db.JournalEntries.FindAsync(id);
            if (entry != null)
            {
                entry.Title = title;
                entry.Content = content;
                entry.Date = DateTime.UtcNow;
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