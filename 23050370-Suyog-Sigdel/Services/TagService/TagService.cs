using _23050370_Suyog_Sigdel.Data;
using _23050370_Suyog_Sigdel.Models;
using Microsoft.EntityFrameworkCore;

namespace _23050370_Suyog_Sigdel.Services.TagService;

public class TagService
{
    private readonly AppDbContext _db;

    public TagService(AppDbContext db)
    {
        _db = db;
    }

    // ──────────────────────────────────────────────
    // GET ALL TAGS
    // ──────────────────────────────────────────────
    public async Task<List<TagModel>> GetAllTagsAsync()
    {
        try
        {
            return await _db.Tags
                .OrderBy(t => t.Name)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Get Tags Error: " + ex.Message);
            return new List<TagModel>();
        }
    }

    // ──────────────────────────────────────────────
    // GET OR CREATE TAG BY NAME
    // ──────────────────────────────────────────────
    public async Task<TagModel> GetOrCreateTagAsync(string tagName)
    {
        try
        {
            tagName = tagName.Trim().ToLower();
            
            var existingTag = await _db.Tags
                .FirstOrDefaultAsync(t => t.Name == tagName);

            if (existingTag != null)
                return existingTag;

            var newTag = new TagModel { Name = tagName };
            await _db.Tags.AddAsync(newTag);
            await _db.SaveChangesAsync();
            return newTag;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Get/Create Tag Error: " + ex.Message);
            return new TagModel { Name = tagName };
        }
    }

    // ──────────────────────────────────────────────
    // DELETE TAG (if not used)
    // ──────────────────────────────────────────────
    public async Task<bool> DeleteTagAsync(int tagId)
    {
        try
        {
            var tag = await _db.Tags
                .Include(t => t.JournalEntries)
                .FirstOrDefaultAsync(t => t.Id == tagId);

            if (tag == null) return false;

            // Only delete if not used in any entries
            if (tag.JournalEntries.Count == 0)
            {
                _db.Tags.Remove(tag);
                await _db.SaveChangesAsync();
                return true;
            }

            return false; // Tag is in use
        }
        catch (Exception ex)
        {
            Console.WriteLine("Delete Tag Error: " + ex.Message);
            return false;
        }
    }

    // ──────────────────────────────────────────────
    // GET TAGS FOR ENTRY
    // ──────────────────────────────────────────────
    public async Task<List<TagModel>> GetTagsForEntryAsync(int entryId)
    {
        try
        {
            var entry = await _db.JournalEntries
                .Include(e => e.Tags)
                .FirstOrDefaultAsync(e => e.Id == entryId);

            return entry?.Tags ?? new List<TagModel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Get Entry Tags Error: " + ex.Message);
            return new List<TagModel>();
        }
    }
}
