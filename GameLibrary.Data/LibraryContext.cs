using GameLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GameLibrary.Data;

public class LibraryContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Publisher> Publishers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=gamelibrary.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // SQLite погано працює з decimal, тому зберігаємо ціну як double
        modelBuilder.Entity<Game>()
            .Property(g => g.Price)
            .HasConversion<double>();

        modelBuilder.Entity<Game>()
            .Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<Genre>()
            .HasIndex(g => g.Name)
            .IsUnique();
    }
}