using _23050370_Suyog_Sigdel.Services;
using _23050370_Suyog_Sigdel.Data;
using _23050370_Suyog_Sigdel.Services.StreakService;
using _23050370_Suyog_Sigdel.Services.TagService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace _23050370_Suyog_Sigdel;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // ----------------------------------------------------
        // MAUI APP CONFIGURATION
        // ----------------------------------------------------
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // ----------------------------------------------------
        // DATABASE CONFIGURATION (SQLite)
        // ----------------------------------------------------
        // SQLite database path inside app data folder
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "journals.db");
        Console.WriteLine($"SQLite DB Path: {dbPath}"); // For checking Db path

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite($"Data Source={dbPath}");
            options.LogTo(Console.WriteLine); // EF Core logs
        });

        // ----------------------------------------------------
        // DEPENDENCY INJECTION (SERVICES)
        // ----------------------------------------------------
        builder.Services.AddScoped<JournalService>();
        builder.Services.AddScoped<TagService>();
        builder.Services.AddSingleton<ThemeService>();
        builder.Services.AddSingleton<SearchService>();
        builder.Services.AddScoped<StreakService>();
        builder.Services.AddScoped<AnalyticsService>();


        
        builder.Services.AddSingleton<SecurityService>(serviceProvider => 
        {
            //  Single instance for entire app (As app was running in continuous loop)
            var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return new SecurityService(dbContext);
        });


        // ----------------------------------------------------
        // BLAZOR WEBVIEW
        // ----------------------------------------------------
        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        // ----------------------------------------------------
        // DEBUG & DEVELOPER TOOLS
        // ----------------------------------------------------
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // ----------------------------------------------------
        // ENSURE DATABASE IS CREATED
        // ----------------------------------------------------
        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated(); // Creates SQLite DB if not exists
        }

        return app;
    }
}
