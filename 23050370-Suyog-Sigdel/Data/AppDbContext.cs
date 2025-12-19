using Microsoft.EntityFrameworkCore;
using _23050370_Suyog_Sigdel.Models;

namespace _23050370_Suyog_Sigdel.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.ToTable("journal_entries");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Content)
                .HasColumnName("content")
                .IsRequired();

            entity.Property(e => e.Date)
                .HasColumnName("date")
                .IsRequired();

            // DB-generated column (trigger)
            entity.Property(e => e.EntryDay)
                .HasColumnName("entry_day")
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.SetAfterSaveBehavior(
                    Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore
                );
        });
    }
}