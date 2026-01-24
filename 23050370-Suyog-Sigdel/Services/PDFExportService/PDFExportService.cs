using _23050370_Suyog_Sigdel.Data;
using _23050370_Suyog_Sigdel.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace _23050370_Suyog_Sigdel.Services.PDFExportService;

/// 
/// Simple PDF export service - generates HTML that can be printed to PDF
///
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
        var entries = await _db.JournalEntries
            .Include(e => e.Tags)
            .Where(e => e.EntryDay >= startDate && e.EntryDay <= endDate)
            .OrderBy(e => e.EntryDay)
            .ToListAsync();

        if (!entries.Any())
        {
            return string.Empty;
        }

        // Create HTML file that can be printed to PDF
        var fileName = $"Journal_Export_{startDate:yyyy-MM-dd}_to_{endDate:yyyy-MM-dd}.html";
        var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        // Generate printable HTML
        await GenerateHtmlAsync(entries, filePath, startDate, endDate);

        return filePath;
    }

    // Export single entry
    public async Task<string> ExportSingleEntryAsync(int entryId)
    {
        var entry = await _db.JournalEntries
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.Id == entryId);

        if (entry == null)
        {
            return string.Empty;
        }

        var fileName = $"Journal_Entry_{entry.EntryDay:yyyy-MM-dd}.html";
        var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        await GenerateHtmlAsync(new List<JournalEntryModel> { entry }, filePath, entry.EntryDay, entry.EntryDay);

        return filePath;
    }

    // Generate HTML file (can be opened in browser and printed to PDF)
    private async Task GenerateHtmlAsync(List<JournalEntryModel> entries, string outputPath, DateOnly startDate, DateOnly endDate)
    {
        var sb = new StringBuilder();

        // HTML header with print-friendly styles
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("    <meta charset=\"UTF-8\">");
        sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        sb.AppendLine($"    <title>Journal Export - {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}</title>");
        sb.AppendLine("    <style>");
        sb.AppendLine(GetPrintStyles());
        sb.AppendLine("    </style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");

        // Cover page
        sb.AppendLine("    <div class=\"cover-page\">");
        sb.AppendLine("        <h1>My Journal</h1>");
        sb.AppendLine($"        <p class=\"date-range\">{startDate.ToString("MMMM dd, yyyy")} - {endDate.ToString("MMMM dd, yyyy")}</p>");
        sb.AppendLine($"        <p class=\"entry-count\">{entries.Count} {(entries.Count == 1 ? "Entry" : "Entries")}</p>");
        sb.AppendLine("    </div>");

        // Entries
        foreach (var entry in entries)
        {
            sb.AppendLine("    <div class=\"entry-page\">");
            
            // Title
            if (!string.IsNullOrWhiteSpace(entry.Title))
            {
                sb.AppendLine($"        <h2 class=\"entry-title\">{EscapeHtml(entry.Title)}</h2>");
            }

            // Date
            sb.AppendLine($"        <p class=\"entry-date\">{entry.EntryDay.ToString("dddd, MMMM dd, yyyy")}</p>");

            // Moods
            if (!string.IsNullOrEmpty(entry.PrimaryMood))
            {
                sb.AppendLine("        <div class=\"mood-section\">");
                sb.Append($"            <span class=\"mood-label\">Mood:</span> <span class=\"mood-primary\">{EscapeHtml(entry.PrimaryMood)}</span>");
                
                if (!string.IsNullOrEmpty(entry.SecondaryMood1))
                {
                    sb.Append($", <span class=\"mood-secondary\">{EscapeHtml(entry.SecondaryMood1)}</span>");
                }
                
                if (!string.IsNullOrEmpty(entry.SecondaryMood2))
                {
                    sb.Append($", <span class=\"mood-secondary\">{EscapeHtml(entry.SecondaryMood2)}</span>");
                }
                
                sb.AppendLine();
                sb.AppendLine("        </div>");
            }

            // Tags
            if (entry.Tags.Any())
            {
                sb.AppendLine("        <div class=\"tags-section\">");
                sb.Append("            <span class=\"tag-label\">Tags:</span> ");
                sb.Append(string.Join(", ", entry.Tags.Select(t => $"<span class=\"tag\">{EscapeHtml(t.Name)}</span>")));
                sb.AppendLine();
                sb.AppendLine("        </div>");
            }

            // Content
            sb.AppendLine("        <div class=\"entry-content\">");
            sb.AppendLine($"            {entry.Content}");
            sb.AppendLine("        </div>");

            sb.AppendLine("    </div>");
        }

        // Footer with instructions
        sb.AppendLine("    <div class=\"print-instructions\">");
        sb.AppendLine("        <p><strong>To save as PDF:</strong></p>");
        sb.AppendLine("        <ol>");
        sb.AppendLine("            <li>Open this file in a web browser (Safari, Chrome, etc.)</li>");
        sb.AppendLine("            <li>Press Cmd+P (Mac) or Ctrl+P (Windows) to print</li>");
        sb.AppendLine("            <li>Select 'Save as PDF' as the destination</li>");
        sb.AppendLine("            <li>Click Save</li>");
        sb.AppendLine("        </ol>");
        sb.AppendLine("    </div>");

        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        // Write to file
        await File.WriteAllTextAsync(outputPath, sb.ToString());
    }

    // Get CSS styles for print-friendly HTML
    private string GetPrintStyles()
    {
        return @"
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Arial, sans-serif;
            line-height: 1.6;
            color: #2c3e50;
            max-width: 800px;
            margin: 0 auto;
            padding: 2rem;
            background: #f8f9fa;
        }

        .cover-page {
            text-align: center;
            padding: 4rem 2rem;
            background: white;
            border-radius: 8px;
            margin-bottom: 2rem;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }

        .cover-page h1 {
            font-size: 2.5rem;
            color: #3498db;
            margin-bottom: 1rem;
        }

        .date-range {
            font-size: 1.2rem;
            color: #7f8c8d;
            margin-bottom: 0.5rem;
        }

        .entry-count {
            font-size: 1rem;
            color: #95a5a6;
        }

        .entry-page {
            background: white;
            padding: 2rem;
            margin-bottom: 2rem;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            page-break-after: always;
        }

        .entry-title {
            font-size: 1.8rem;
            color: #2c3e50;
            margin-bottom: 0.5rem;
        }

        .entry-date {
            font-size: 1rem;
            color: #7f8c8d;
            font-weight: 500;
            margin-bottom: 1rem;
        }

        .mood-section,
        .tags-section {
            margin-bottom: 1rem;
            padding: 0.5rem 0;
        }

        .mood-label,
        .tag-label {
            font-weight: 600;
            color: #34495e;
        }

        .mood-primary {
            color: #3498db;
            font-weight: 600;
        }

        .mood-secondary {
            color: #5dade2;
        }

        .tag {
            background: #e7f3ff;
            color: #3498db;
            padding: 0.2rem 0.6rem;
            border-radius: 12px;
            font-size: 0.9rem;
            display: inline-block;
            margin: 0.2rem;
        }

        .entry-content {
            margin-top: 1.5rem;
            line-height: 1.8;
        }

        .entry-content h1,
        .entry-content h2,
        .entry-content h3 {
            margin-top: 1rem;
            margin-bottom: 0.5rem;
            color: #2c3e50;
        }

        .entry-content p {
            margin-bottom: 0.8rem;
        }

        .entry-content ul,
        .entry-content ol {
            margin-left: 2rem;
            margin-bottom: 0.8rem;
        }

        .print-instructions {
            background: #fff3cd;
            border: 2px solid #ffc107;
            padding: 1.5rem;
            border-radius: 8px;
            margin-top: 2rem;
        }

        .print-instructions strong {
            color: #856404;
        }

        .print-instructions ol {
            margin-left: 2rem;
            margin-top: 0.5rem;
        }

        .print-instructions li {
            margin-bottom: 0.3rem;
        }

        /* Print-specific styles */
        @media print {
            body {
                background: white;
                padding: 0;
            }

            .print-instructions {
                display: none;
            }

            .entry-page {
                box-shadow: none;
                border: none;
            }
        }
        ";
    }

    // Escape HTML special characters
    private string EscapeHtml(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#39;");
    }
}
