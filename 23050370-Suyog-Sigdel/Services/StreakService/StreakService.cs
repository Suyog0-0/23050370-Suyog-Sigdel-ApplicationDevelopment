using _23050370_Suyog_Sigdel.Data;
using Microsoft.EntityFrameworkCore;

namespace _23050370_Suyog_Sigdel.Services.StreakService;

public class StreakService(AppDbContext db)
{
    // Calculate current streak
    public async Task<int> GetCurrentStreakAsync()
    {
        var entries = await db.JournalEntries
            .OrderByDescending(e => e.EntryDay)
            .Select(e => e.EntryDay)
            .ToListAsync();

        if (!entries.Any()) return 0;

        var today = DateOnly.FromDateTime(DateTime.Now);
        var yesterday = today.AddDays(-1);

        // Check if there's an entry today or yesterday
        if (entries[0] != today && entries[0] != yesterday)
            return 0;

        int streak = 0;
        var currentDay = entries[0] == today ? today : yesterday;

        foreach (var entryDay in entries)
        {
            if (entryDay == currentDay)
            {
                streak++;
                currentDay = currentDay.AddDays(-1);
            }
            else if (entryDay < currentDay)
            {
                // Gap found, streak broken
                break;
            }
        }

        return streak;
    }

    // Calculate longest streak ever
    public async Task<int> GetLongestStreakAsync()
    {
        var entries = await db.JournalEntries
            .OrderBy(e => e.EntryDay)
            .Select(e => e.EntryDay)
            .ToListAsync();

        if (!entries.Any()) return 0;

        int longestStreak = 1;
        int currentStreak = 1;

        for (int i = 1; i < entries.Count; i++)
        {
            // Calculate days difference
            var daysDiff = (entries[i].ToDateTime(TimeOnly.MinValue) - entries[i - 1].ToDateTime(TimeOnly.MinValue)).Days;

            if (daysDiff == 1)
            {
                // Consecutive day
                currentStreak++;
                if (currentStreak > longestStreak)
                    longestStreak = currentStreak;
            }
            else
            {
                // Gap found, reset current streak
                currentStreak = 1;
            }
        }

        return longestStreak;
    }

    // Calculate total entries
    public async Task<int> GetTotalEntriesAsync()
    {
        return await db.JournalEntries.CountAsync();
    }

    // Calculate missed days (days between first entry and today with no entry)
    public async Task<int> GetMissedDaysAsync()
    {
        var entries = await db.JournalEntries
            .OrderBy(e => e.EntryDay)
            .Select(e => e.EntryDay)
            .ToListAsync();

        if (!entries.Any()) return 0;

        var firstEntry = entries.First();
        var today = DateOnly.FromDateTime(DateTime.Now);
        
        // Calculate total days from first entry to today
        var totalDays = (today.ToDateTime(TimeOnly.MinValue) - firstEntry.ToDateTime(TimeOnly.MinValue)).Days + 1;
        var entriesCount = entries.Count;
        
        return totalDays - entriesCount;
    }
}