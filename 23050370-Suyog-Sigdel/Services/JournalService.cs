using Microsoft.EntityFrameworkCore;
using _23050370_Suyog_Sigdel.Data;
using _23050370_Suyog_Sigdel.Models;

namespace _23050370_Suyog_Sigdel.Services;

public class JournalService
{
    private readonly AppDbContext _db;

    public JournalService(AppDbContext db)
    {
        _db = db;
    }

    // ──────────────────────────────────────────────
    // 🔹 GET ALL JOURNAL ENTRIES
    // ──────────────────────────────────────────────
    public async Task<List<JournalEntry>> GetEntriesAsync()
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
            return new List<JournalEntry>();
        }
    }

    // ──────────────────────────────────────────────
    // 🔹 GET ENTRY BY DATE
    // ──────────────────────────────────────────────
    public async Task<JournalEntry?> GetEntryByDateAsync(DateOnly date)
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

    // ──────────────────────────────────────────────
    // 🔹 ADD OR UPDATE ENTRY FOR TODAY
    // ──────────────────────────────────────────────
    public async Task AddOrUpdateEntryAsync(string content)
    {
        try
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var existingEntry = await GetEntryByDateAsync(today);

            if (existingEntry != null)
            {
                existingEntry.Content = content;
                existingEntry.Date = DateTime.UtcNow;
                _db.JournalEntries.Update(existingEntry);
            }
            else
            {
                var entry = new JournalEntry
                {
                    Date = DateTime.UtcNow,
                    EntryDay = today,
                    Content = content
                };
                await _db.JournalEntries.AddAsync(entry);
            }

            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("DB Insert/Update Error: " + ex.Message);
        }
    }

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
