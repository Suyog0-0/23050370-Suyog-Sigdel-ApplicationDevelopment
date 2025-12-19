using Microsoft.EntityFrameworkCore;
using _23050370_Suyog_Sigdel.Data;
using _23050370_Suyog_Sigdel.Models;
using _23050370_Suyog_Sigdel.Models;

namespace _23050370_Suyog_Sigdel.Services;

public class JournalService
{
    private readonly AppDbContext _db;

    public JournalService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<JournalEntry>> GetEntriesAsync()
    {
        try
        {
            return await _db.JournalEntries.OrderByDescending(e => e.Date).ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("DB Connection Error: " + ex.Message);
            return new List<JournalEntry>(); // Avoid UI hang
        }
    }

    public async Task AddEntryAsync(string content)
    {
        try
        {
            var entry = new JournalEntry
            {
                Date = DateTime.UtcNow,
                Content = content
            };

            _db.JournalEntries.Add(entry);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("DB Insert Error: " + ex.Message);
        }
    }
}