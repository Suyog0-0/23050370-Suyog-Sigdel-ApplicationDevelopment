using _23050370_Suyog_Sigdel.Data;
using _23050370_Suyog_Sigdel.Models;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using System.Text.RegularExpressions;

namespace _23050370_Suyog_Sigdel.Services.PDFExportService;

public class PDFExportService
{
    private readonly AppDbContext _db;

    public PDFExportService(AppDbContext db)
    {
        _db = db;
    }

    // Export entries by date range
    public async Task<string> ExportEntriesByDateRangeAsync(DateOnly startDate, DateOnly endDate)
    {
        try
        {
            var entries = await _db.JournalEntries
                .Include(e => e.Tags)
                .Where(e => e.EntryDay >= startDate && e.EntryDay <= endDate)
                .OrderBy(e => e.EntryDay)
                .ToListAsync();

            if (!entries.Any())
            {
                return string.Empty;
            }

            var fileName = $"Journal_Export_{startDate:yyyy-MM-dd}_to_{endDate:yyyy-MM-dd}.pdf";
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            GeneratePdf(entries, filePath, startDate, endDate);

            return filePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Export Error: {ex.Message}");
            throw;
        }
    }

    // Export single entry
    public async Task<string> ExportSingleEntryAsync(int entryId)
    {
        var entry = await _db.JournalEntries
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.Id == entryId);

        if (entry == null)
            return string.Empty;

        var fileName = $"Journal_Entry_{entry.EntryDay:yyyy-MM-dd}.pdf";
        var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        GeneratePdf(new List<JournalEntryModel> { entry }, filePath, entry.EntryDay, entry.EntryDay);

        return filePath;
    }

    private void GeneratePdf(List<JournalEntryModel> entries, string outputPath, DateOnly startDate, DateOnly endDate)
    {
        using (var writer = new PdfWriter(outputPath))
        using (var pdf = new PdfDocument(writer))
        using (var document = new Document(pdf))
        {
            // Title
            var title = new Paragraph($"Journal Export")
                .SetFontSize(24)
                .SetBold()
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetMarginBottom(10);
            document.Add(title);

            // Date range
            var dateRange = new Paragraph($"{startDate:MMMM dd, yyyy} - {endDate:MMMM dd, yyyy}")
                .SetFontSize(14)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetMarginBottom(5);
            document.Add(dateRange);

            // Entry count
            var count = new Paragraph($"{entries.Count} {(entries.Count == 1 ? "Entry" : "Entries")}")
                .SetFontSize(12)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetMarginBottom(30);
            document.Add(count);

            // Add each entry
            foreach (var entry in entries)
            {
                // Title
                if (!string.IsNullOrWhiteSpace(entry.Title))
                {
                    var entryTitle = new Paragraph(entry.Title)
                        .SetFontSize(18)
                        .SetBold()
                        .SetMarginBottom(5);
                    document.Add(entryTitle);
                }

                // Date
                var entryDate = new Paragraph(entry.EntryDay.ToString("dddd, MMMM dd, yyyy"))
                    .SetFontSize(12)
                    .SetFontColor(ColorConstants.DARK_GRAY)
                    .SetMarginBottom(10);
                document.Add(entryDate);

                // Moods
                if (!string.IsNullOrEmpty(entry.PrimaryMood))
                {
                    var moods = $"Mood: {entry.PrimaryMood}";
                    if (!string.IsNullOrEmpty(entry.SecondaryMood1))
                        moods += $", {entry.SecondaryMood1}";
                    if (!string.IsNullOrEmpty(entry.SecondaryMood2))
                        moods += $", {entry.SecondaryMood2}";

                    var moodPara = new Paragraph(moods)
                        .SetFontSize(11)
                        .SetFontColor(ColorConstants.BLUE)
                        .SetMarginBottom(5);
                    document.Add(moodPara);
                }

                // Tags
                if (entry.Tags.Any())
                {
                    var tags = $"Tags: {string.Join(", ", entry.Tags.Select(t => t.Name))}";
                    var tagPara = new Paragraph(tags)
                        .SetFontSize(11)
                        .SetFontColor(ColorConstants.BLUE)
                        .SetMarginBottom(10);
                    document.Add(tagPara);
                }

                // Content
                var plainText = StripHtml(entry.Content);
                var content = new Paragraph(plainText)
                    .SetFontSize(11)
                    .SetMarginBottom(30);
                document.Add(content);

                // Add page break between entries (except last)
                if (entry != entries.Last())
                {
                    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                }
            }
        }
    }

    private string StripHtml(string html)
    {
        if (string.IsNullOrEmpty(html))
            return string.Empty;

        var text = Regex.Replace(html, "<.*?>", " ");
        text = System.Net.WebUtility.HtmlDecode(text);
        text = Regex.Replace(text, @"\s+", " ").Trim();
        
        return text;
    }
}