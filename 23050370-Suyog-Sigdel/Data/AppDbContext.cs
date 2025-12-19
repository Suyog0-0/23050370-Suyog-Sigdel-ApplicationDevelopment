using Microsoft.EntityFrameworkCore;
using _23050370_Suyog_Sigdel.Models;

namespace _23050370_Suyog_Sigdel.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<JournalEntry> JournalEntries { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.ToTable("journal_entries");  // map to the exact table name as in supabase

            entity.HasKey(e => e.Id);

            // Map all columns to lowercase
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Content).HasColumnName("content");
        });
    }

}