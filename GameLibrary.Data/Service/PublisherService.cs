using GameLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Data.Services;

public class PublisherService
{
    private readonly LibraryContext _db;

    public PublisherService(LibraryContext db) => _db = db;

    // CREATE
    public Publisher Create(Publisher publisher)
    {
        _db.Publishers.Add(publisher);
        _db.SaveChanges();
        return publisher;
    }

    // READ (всі)
    public List<Publisher> GetAll() => _db.Publishers.ToList();

    // READ (один)
    public Publisher? GetById(int id) => _db.Publishers.Find(id);

    // UPDATE
    public bool Update(Publisher updated)
    {
        var existing = _db.Publishers.Find(updated.Id);
        if (existing == null) return false;

        existing.Name = updated.Name;
        existing.Country = updated.Country;
        existing.FoundedYear = updated.FoundedYear;
        existing.Website = updated.Website;
        existing.Email = updated.Email;

        _db.SaveChanges();
        return true;
    }

    // DELETE
    public bool Delete(int id)
    {
        var existing = _db.Publishers.Find(id);
        if (existing == null) return false;

        _db.Publishers.Remove(existing);
        _db.SaveChanges();
        return true;
    }

    // READ з EXPLICIT loading: ігри видавця підвантажуються окремим запитом
    public Publisher? GetByIdWithGames(int id)
    {
        var publisher = _db.Publishers.Find(id);
        if (publisher == null) return null;

        _db.Entry(publisher).Collection(p => p.Games).Load();
        return publisher;
    }

    // Усі ігри вказаного видавця (за Id видавця), explicit loading
    public List<Game> GetGames(int publisherId)
    {
        var publisher = _db.Publishers.Find(publisherId);
        if (publisher == null) return new List<Game>();

        _db.Entry(publisher).Collection(p => p.Games).Load();
        return publisher.Games;
    }
}

