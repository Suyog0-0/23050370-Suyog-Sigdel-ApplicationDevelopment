using _23050370_Suyog_Sigdel.Data;

namespace _23050370_Suyog_Sigdel.Services;

public partial class JournalService
{
    // ──────────────────────────────────────────────
    // 🔹 DELETE ENTRY BY ID
    // ──────────────────────────────────────────────
    public async Task DeleteEntryAsync(int id)
    {
        try
        {
            var entry = await _db.JournalEntries.FindAsync(id);
            if (entry != null)
            {
                _db.JournalEntries.Remove(entry);
                await _db.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("DB Delete Error: " + ex.Message);
        }
    }
}