using _23050370_Suyog_Sigdel.Data;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace _23050370_Suyog_Sigdel.Services.PDFExportService;

public class PDFExportService
{
    private readonly AppDbContext _db;

    public PDFExportService(AppDbContext db)
    {
        _db = db;
    }

    // Export entries by date range
    public async Task<(int count, string folderPath)> ExportByDateRangeAsync(DateOnly startDate, DateOnly endDate)
    {
        try
        {
            Console.WriteLine($"[PDF Export] Start Date: {startDate}");
            Console.WriteLine($"[PDF Export] End Date: {endDate}");

            // First, get ALL entries to see what's in database
            var allEntries = await _db.JournalEntries.ToListAsync();
            Console.WriteLine($"[PDF Export] Total entries in database: {allEntries.Count}");
            
            foreach (var e in allEntries)
            {
                Console.WriteLine($"[PDF Export] DB Entry - Date: {e.EntryDay}, Title: '{e.Title}'");
            }

            // Get entries in date range
            var entries = await _db.JournalEntries
                .Include(e => e.Tags)
                .Where(e => e.EntryDay >= startDate && e.EntryDay <= endDate)
                .OrderBy(e => e.EntryDay)
                .ToListAsync();

            Console.WriteLine($"[PDF Export] Found {entries.Count} entries in range");
            
            foreach (var entry in entries)
            {
                Console.WriteLine($"[PDF Export] Matched Entry - Date: {entry.EntryDay}, Title: {entry.Title}");
            }

            if (!entries.Any())
            {
                Console.WriteLine("[PDF Export] No entries found - returning empty");
                return (0, string.Empty);
            }

            // Create folder for exports
            var folderName = $"Journal_Export_{startDate:yyyy-MM-dd}_to_{endDate:yyyy-MM-dd}";
            var folderPath = Path.Combine(FileSystem.AppDataDirectory, folderName);
            
            Console.WriteLine($"[PDF Export] Folder Path: {folderPath}");
            
            // Delete folder if it exists and create new one
            if (Directory.Exists(folderPath))
            {
                Console.WriteLine("[PDF Export] Deleting existing folder");
                Directory.Delete(folderPath, true);
            }
            
            Directory.CreateDirectory(folderPath);
            Console.WriteLine("[PDF Export] Folder created");

            // Create PDF for each entry
            int count = 0;
            foreach (var entry in entries)
            {
                var fileName = $"{entry.EntryDay:yyyy-MM-dd}_{SanitizeFileName(entry.Title)}.pdf";
                var filePath = Path.Combine(folderPath, fileName);
                
                Console.WriteLine($"[PDF Export] Creating PDF: {fileName}");
                CreatePDF(entry, filePath);
                count++;
            }

            Console.WriteLine($"[PDF Export] Successfully created {count} PDFs");
            return (count, folderPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PDF Export] Error: {ex.Message}");
            Console.WriteLine($"[PDF Export] Stack Trace: {ex.StackTrace}");
            throw;
        }
    }

    // Create single PDF file
    private void CreatePDF(Models.JournalEntryModel entry, string filePath)
    {
        using var writer = new PdfWriter(filePath);
        using var pdf = new PdfDocument(writer);
        using var document = new Document(pdf);

        // Title
        if (!string.IsNullOrWhiteSpace(entry.Title))
        {
            document.Add(new Paragraph(entry.Title)
                .SetFontSize(18)
                .SetBold());
        }

        // Date
        document.Add(new Paragraph(entry.EntryDay.ToString("dddd, MMMM dd, yyyy"))
            .SetFontSize(12)
            .SetMarginBottom(10));

        // Moods
        if (!string.IsNullOrEmpty(entry.PrimaryMood))
        {
            var moodText = $"Mood: {entry.PrimaryMood}";
            if (!string.IsNullOrEmpty(entry.SecondaryMood1))
                moodText += $", {entry.SecondaryMood1}";
            if (!string.IsNullOrEmpty(entry.SecondaryMood2))
                moodText += $", {entry.SecondaryMood2}";
            
            document.Add(new Paragraph(moodText)
                .SetFontSize(11)
                .SetMarginBottom(5));
        }

        // Tags
        if (entry.Tags.Any())
        {
            var tagText = $"Tags: {string.Join(", ", entry.Tags.Select(t => t.Name))}";
            document.Add(new Paragraph(tagText)
                .SetFontSize(11)
                .SetMarginBottom(15));
        }

        // Content (strip HTML)
        var plainText = StripHtml(entry.Content);
        document.Add(new Paragraph(plainText)
            .SetFontSize(11));
    }

    // Remove HTML tags
    private string StripHtml(string html)
    {
        if (string.IsNullOrEmpty(html)) return string.Empty;
        
        var text = System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", " ");
        text = System.Net.WebUtility.HtmlDecode(text);
        text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim();
        
        return text;
    }

    // Sanitize filename
    private string SanitizeFileName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "Untitled";
        
        var invalid = Path.GetInvalidFileNameChars();
        var sanitized = string.Join("_", name.Split(invalid));
        
        return sanitized.Length > 50 ? sanitized.Substring(0, 50) : sanitized;
    }
}
