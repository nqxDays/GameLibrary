using GameLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Data.Services;

public class GameService
{
    private readonly LibraryContext _db;

    public GameService(LibraryContext db) => _db = db;

    // CREATE: жанри передаються за Id
    public Game Create(Game game, List<int> genreIds)
    {
        var genres = _db.Genres.Where(g => genreIds.Contains(g.Id)).ToList();
        game.Genres.AddRange(genres);

        _db.Games.Add(game);
        _db.SaveChanges();
        return game;
    }

    // READ (всі), EAGER loading: жанри завантажуються разом з іграми
    public List<Game> GetAll() =>
        _db.Games.Include(g => g.Genres).ToList();

    // READ (одна), EAGER loading для жанрів
    public Game? GetById(int id) =>
        _db.Games.Include(g => g.Genres).FirstOrDefault(g => g.Id == id);

    // READ (одна), EXPLICIT loading: видавець підвантажується окремим запитом
    public Game? GetByIdWithPublisher(int id)
    {
        var game = _db.Games.Find(id);
        if (game == null) return null;

        _db.Entry(game).Reference(g => g.Publisher).Load();
        return game;
    }

    // UPDATE: оновлює поля і список жанрів
    public bool Update(Game updated, List<int> genreIds)
    {
        var existing = _db.Games
            .Include(g => g.Genres)
            .FirstOrDefault(g => g.Id == updated.Id);
        if (existing == null) return false;

        existing.Title = updated.Title;
        existing.Price = updated.Price;
        existing.ReleaseYear = updated.ReleaseYear;
        existing.Rating = updated.Rating;
        existing.Description = updated.Description;
        existing.PublisherId = updated.PublisherId;

        existing.Genres.Clear();
        existing.Genres.AddRange(_db.Genres.Where(g => genreIds.Contains(g.Id)));

        _db.SaveChanges();
        return true;
    }

    // DELETE
    public bool Delete(int id)
    {
        var existing = _db.Games.Find(id);
        if (existing == null) return false;

        _db.Games.Remove(existing);
        _db.SaveChanges();
        return true;
    }
    // Усі ігри вказаного жанру (за назвою жанру)
    public List<Game> GetByGenre(string genreName) =>
        _db.Games
            .Include(g => g.Genres)
            .Where(g => g.Genres.Any(genre => genre.Name == genreName))
            .ToList();
}