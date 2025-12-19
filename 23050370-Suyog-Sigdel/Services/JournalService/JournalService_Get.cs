
using Microsoft.EntityFrameworkCore;
using _23050370_Suyog_Sigdel.Models;

namespace _23050370_Suyog_Sigdel.Services;

public partial class JournalService
{
    // 🔹 GET ALL JOURNAL ENTRIES
    public async Task<List<JournalEntry_Model>> GetEntriesAsync()
    {
        try
        {
            return await _db.JournalEntries
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("DB Connection Error: " + ex.Message);
            return new List<JournalEntry_Model>();
        }
    }

    // 🔹 GET ENTRY BY DATE
    public async Task<JournalEntry_Model?> GetEntryByDateAsync(DateOnly date)
    {
        try
        {
            return await _db.JournalEntries
                .FirstOrDefaultAsync(e => e.EntryDay == date);
        }
        catch (Exception ex)
        {
            Console.WriteLine("DB Fetch Error: " + ex.Message);
            return null;
        }
    }
}
