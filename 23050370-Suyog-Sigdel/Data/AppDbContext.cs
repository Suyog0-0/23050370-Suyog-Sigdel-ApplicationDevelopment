using Microsoft.EntityFrameworkCore;
using _23050370_Suyog_Sigdel.Models;

namespace _23050370_Suyog_Sigdel.Data
{
    public class AppDbContext : DbContext
    {
        // Using correct model class
        public DbSet<JournalEntryModel> JournalEntries { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Convert DateOnly to string for SQLite
            modelBuilder.Entity<JournalEntryModel>()
                .Property<DateOnly>(j => j.EntryDay)
                .HasConversion<string>(
                    v => v.ToString(),       // store as string
                    v => DateOnly.Parse(v)   // convert back to DateOnly
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}