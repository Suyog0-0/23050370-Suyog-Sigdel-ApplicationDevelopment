using _23050370_Suyog_Sigdel.Models;

namespace _23050370_Suyog_Sigdel.Services;

public partial class JournalService
{
    // 🔹 ADD NEW ENTRY
    public async Task AddEntryAsync(string content)
    {
        try
        {
            var entry = new JournalEntry_Model
            {
                Date = DateTime.UtcNow,
                EntryDay = DateOnly.FromDateTime(DateTime.UtcNow),
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