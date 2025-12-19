using _23050370_Suyog_Sigdel.Models;

namespace _23050370_Suyog_Sigdel.Services;

public partial class JournalService
{
    // 🔹 UPDATE ENTRY
    public async Task UpdateEntryAsync(string content)
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
                await _db.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("No entry found for today. Use Add first.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("DB Update Error: " + ex.Message);
        }
    }
}