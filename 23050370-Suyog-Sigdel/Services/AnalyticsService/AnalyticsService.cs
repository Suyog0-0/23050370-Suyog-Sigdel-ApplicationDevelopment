using _23050370_Suyog_Sigdel.Data;
using Microsoft.EntityFrameworkCore;

namespace _23050370_Suyog_Sigdel.Services;

public class AnalyticsService
{
    private readonly AppDbContext _db;

    public AnalyticsService(AppDbContext db)
    {
        _db = db;
    }

    // Get mood distribution (count of each mood)
    public async Task<Dictionary<string, int>> GetMoodDistributionAsync()
    {
        var entries = await _db.JournalEntries.ToListAsync();
        
        var moodCounts = new Dictionary<string, int>();

        foreach (var entry in entries)
        {
            // Count primary mood
            if (!string.IsNullOrEmpty(entry.PrimaryMood))
            {
                if (moodCounts.ContainsKey(entry.PrimaryMood))
                    moodCounts[entry.PrimaryMood]++;
                else
                    moodCounts[entry.PrimaryMood] = 1;
            }

            // Count secondary mood 1
            if (!string.IsNullOrEmpty(entry.SecondaryMood1))
            {
                if (moodCounts.ContainsKey(entry.SecondaryMood1))
                    moodCounts[entry.SecondaryMood1]++;
                else
                    moodCounts[entry.SecondaryMood1] = 1;
            }

            // Count secondary mood 2
            if (!string.IsNullOrEmpty(entry.SecondaryMood2))
            {
                if (moodCounts.ContainsKey(entry.SecondaryMood2))
                    moodCounts[entry.SecondaryMood2]++;
                else
                    moodCounts[entry.SecondaryMood2] = 1;
            }
        }

        return moodCounts.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
    }

    // Get most frequent moods (top 5)
    public async Task<List<(string Mood, int Count)>> GetTopMoodsAsync(int topCount = 5)
    {
        var distribution = await GetMoodDistributionAsync();
        
        return distribution
            .OrderByDescending(x => x.Value)
            .Take(topCount)
            .Select(x => (x.Key, x.Value))
            .ToList();
    }

    // Get tag distribution (count of each tag)
    public async Task<Dictionary<string, int>> GetTagDistributionAsync()
    {
        var tags = await _db.Tags
            .Include(t => t.JournalEntries)
            .ToListAsync();

        var tagCounts = new Dictionary<string, int>();

        foreach (var tag in tags)
        {
            tagCounts[tag.Name] = tag.JournalEntries.Count;
        }

        return tagCounts.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
    }

    // Get most used tags (top 10)
    public async Task<List<(string Tag, int Count)>> GetTopTagsAsync(int topCount = 10)
    {
        var distribution = await GetTagDistributionAsync();
        
        return distribution
            .OrderByDescending(x => x.Value)
            .Take(topCount)
            .Select(x => (x.Key, x.Value))
            .ToList();
    }

    // Get word count trends (entries with their word counts)
    public async Task<List<(DateOnly Date, int WordCount)>> GetWordCountTrendsAsync()
    {
        var entries = await _db.JournalEntries
            .OrderBy(e => e.EntryDay)
            .ToListAsync();

        var trends = new List<(DateOnly, int)>();

        foreach (var entry in entries)
        {
            // Simple word count: split by spaces and count
            var wordCount = entry.Content
                .Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .Length;

            trends.Add((entry.EntryDay, wordCount));
        }

        return trends;
    }

    // Get average word count
    public async Task<int> GetAverageWordCountAsync()
    {
        var trends = await GetWordCountTrendsAsync();
        
        if (!trends.Any())
            return 0;

        return (int)trends.Average(t => t.WordCount);
    }

    // Get total words written
    public async Task<int> GetTotalWordsAsync()
    {
        var trends = await GetWordCountTrendsAsync();
        return trends.Sum(t => t.WordCount);
    }
}