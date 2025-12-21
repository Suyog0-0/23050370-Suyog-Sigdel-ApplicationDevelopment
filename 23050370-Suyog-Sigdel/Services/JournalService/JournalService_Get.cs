using _23050370_Suyog_Sigdel.Data;
using _23050370_Suyog_Sigdel.Models;
using Microsoft.EntityFrameworkCore;

namespace _23050370_Suyog_Sigdel.Services;

public partial class JournalService
{
    // ──────────────────────────────────────────────
    // GET ALL JOURNAL ENTRIES (NEWEST MODIFIED FIRST)
    // ──────────────────────────────────────────────
    public async Task<List<JournalEntryModel>> GetEntriesAsync()
    {
        try
        {
            return await _db.JournalEntries
                .OrderByDescending(e => e.Date)  // Sort by last modified date (newest first)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("DB Connection Error: " + ex.Message);
            return new List<JournalEntryModel>();
        }
    }

    // ──────────────────────────────────────────────
    // GET ENTRY BY DATE
    // ──────────────────────────────────────────────
    public async Task<JournalEntryModel?> GetEntryByDateAsync(DateOnly date)
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