using Microsoft.EntityFrameworkCore.Design;

namespace GameLibrary.Data;

public class LibraryContextFactory : IDesignTimeDbContextFactory<LibraryContext>
{
    public LibraryContext CreateDbContext(string[] args) => new LibraryContext();
}