using GameLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Data.Services;

public class GenreService
{
    private readonly LibraryContext _db;

    public GenreService(LibraryContext db) => _db = db;

    // CREATE
    public Genre Create(Genre genre)
    {
        _db.Genres.Add(genre);
        _db.SaveChanges();
        return genre;
    }

    // READ (всі), eager loading: разом з іграми
    public List<Genre> GetAll() =>
        _db.Genres.Include(g => g.Games).ToList();

    // READ (один), eager loading
    public Genre? GetById(int id) =>
        _db.Genres.Include(g => g.Games).FirstOrDefault(g => g.Id == id);

    // UPDATE
    public bool Update(Genre updated)
    {
        var existing = _db.Genres.Find(updated.Id);
        if (existing == null) return false;

        existing.Name = updated.Name;
        existing.Description = updated.Description;

        _db.SaveChanges();
        return true;
    }

    // DELETE (записи в проміжній таблиці видаляються автоматично)
    public bool Delete(int id)
    {
        var existing = _db.Genres.Find(id);
        if (existing == null) return false;

        _db.Genres.Remove(existing);
        _db.SaveChanges();
        return true;
    }

    // Усі жанри вказаної гри (за Id гри)
    public List<Genre> GetByGame(int gameId) =>
        _db.Genres
            .Where(genre => genre.Games.Any(g => g.Id == gameId))
            .ToList();
}