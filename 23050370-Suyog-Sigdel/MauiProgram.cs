using _23050370_Suyog_Sigdel.Services;
using _23050370_Suyog_Sigdel.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace _23050370_Suyog_Sigdel;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // =====================================================
        // MAUI APP CONFIGURATION
        // =====================================================
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // =====================================================
        // DATABASE CONFIGURATION (PostgreSQL - Supabase)
        // =====================================================
        string connectionString =
            "Host=db.lsyacsszkbmkgaoxdvgi.supabase.co;" +
            "Database=postgres;" +
            "Username=postgres;" +
            "Password=20132005;" +
            "SSL Mode=Require;Trust Server Certificate=true;Timeout=10;";

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.LogTo(Console.WriteLine); // EF Core SQL & connection logs
        });

        // =====================================================
        // DEPENDENCY INJECTION (SERVICES)
        // =====================================================
        builder.Services.AddScoped<JournalService>();
        builder.Services.AddSingleton<ThemeService>();

        // =====================================================
        // BLAZOR WEBVIEW
        // =====================================================
        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        // =====================================================
        // DEBUG & DEVELOPER TOOLS
        // =====================================================
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}