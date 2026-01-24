using Microsoft.EntityFrameworkCore;
using _23050370_Suyog_Sigdel.Models;

namespace _23050370_Suyog_Sigdel.Data
{
    public class AppDbContext : DbContext
    {
        // Model classes
        public DbSet<JournalEntryModel> JournalEntries { get; set; } = null!;
        public DbSet<SecurityModel> SecuritySettings { get; set; } = null!;
        public DbSet<TagModel> Tags { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Convert DateOnly to string (SQLite)
            modelBuilder.Entity<JournalEntryModel>()
                .Property<DateOnly>(j => j.EntryDay)
                .HasConversion<string>(
                    v => v.ToString(),       // store as string
                    v => DateOnly.Parse(v)   // convert back to DateOnly
                );

            // Many-to-many relationship between JournalEntry and Tag
            modelBuilder.Entity<JournalEntryModel>()
                .HasMany(j => j.Tags)
                .WithMany(t => t.JournalEntries)
                .UsingEntity(j => j.ToTable("journal_entry_tags"));

            base.OnModelCreating(modelBuilder);
        }
    }
}