using _23050370_Suyog_Sigdel.Data;
using _23050370_Suyog_Sigdel.Models;
using Microsoft.EntityFrameworkCore;

namespace _23050370_Suyog_Sigdel.Helpers;

public static class TagHelper
{
    /// 
    /// Gets an existing tag by name or creates a new one if it doesn't exist
    /// 
    public static async Task<TagModel> GetOrCreateTagAsync(AppDbContext db, string tagName)
    {
        tagName = tagName.Trim().ToLower();
        var tag = await db.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
        
        if (tag == null)
        {
            tag = new TagModel { Name = tagName };
            await db.Tags.AddAsync(tag);
        }
        
        return tag;
    }
}