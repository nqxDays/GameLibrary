using GameLibrary.Data;
using GameLibrary.Data.Services;
using GameLibrary.Models;
using Microsoft.EntityFrameworkCore;

Console.OutputEncoding = System.Text.Encoding.UTF8;

using var db = new LibraryContext();
db.Database.Migrate();

var publishers = new PublisherService(db);
var genres = new GenreService(db);
var games = new GameService(db);

// ---------- CREATE ----------
Console.WriteLine("=== CREATE ===");
var cdpr = publishers.Create(new Publisher
{
    Name = "CD Projekt",
    Country = "Польща",
    FoundedYear = 1994,
    Website = "https://cdprojekt.com",
    Email = "info@cdprojekt.com"
});
var valve = publishers.Create(new Publisher
{
    Name = "Valve",
    Country = "США",
    FoundedYear = 1996,
    Website = "https://valvesoftware.com",
    Email = "info@valvesoftware.com"
});

var rpg = genres.Create(new Genre { Name = "RPG", Description = "Рольові ігри" });
var action = genres.Create(new Genre { Name = "Action", Description = "Екшн" });
var puzzle = genres.Create(new Genre { Name = "Puzzle", Description = "Головоломки" });

var witcher = games.Create(new Game
{
    Title = "The Witcher 3",
    Price = 29.99m,
    ReleaseYear = 2015,
    Rating = 9.7,
    Description = "Open world RPG",
    PublisherId = cdpr.Id
}, new List<int> { rpg.Id, action.Id });

var portal = games.Create(new Game
{
    Title = "Portal 2",
    Price = 9.99m,
    ReleaseYear = 2011,
    Rating = 9.5,
    Description = "Puzzle platformer",
    PublisherId = valve.Id
}, new List<int> { puzzle.Id, action.Id });

Console.WriteLine("Дані додано.");

// ---------- READ ----------
Console.WriteLine("\n=== READ: ігри з жанрами (eager loading) ===");
foreach (var g in games.GetAll())
    Console.WriteLine($"{g.Title} - {string.Join(", ", g.Genres.Select(x => x.Name))}");

Console.WriteLine("\n=== READ: гра з видавцем (explicit loading) ===");
var loaded = games.GetByIdWithPublisher(witcher.Id)!;
Console.WriteLine($"{loaded.Title} - {loaded.Publisher.Name}");

Console.WriteLine("\n=== READ: видавець з іграми (explicit loading) ===");
var pub = publishers.GetByIdWithGames(cdpr.Id)!;
Console.WriteLine($"{pub.Name}: {string.Join(", ", pub.Games.Select(x => x.Title))}");

// ---------- UPDATE ----------
Console.WriteLine("\n=== UPDATE ===");
witcher.Price = 19.99m;
games.Update(witcher, new List<int> { rpg.Id });
Console.WriteLine($"Нова ціна: {games.GetById(witcher.Id)!.Price}, " +
                  $"жанри: {string.Join(", ", games.GetById(witcher.Id)!.Genres.Select(x => x.Name))}");

// ---------- Додаткові методи ----------
Console.WriteLine("\n=== Ігри жанру Action ===");
foreach (var g in games.GetByGenre("Action"))
    Console.WriteLine(g.Title);

Console.WriteLine("\n=== Жанри гри Portal 2 ===");
foreach (var g in genres.GetByGame(portal.Id))
    Console.WriteLine(g.Name);

Console.WriteLine("\n=== Ігри видавця Valve ===");
foreach (var g in publishers.GetGames(valve.Id))
    Console.WriteLine(g.Title);

// ---------- DELETE ----------
Console.WriteLine("\n=== DELETE ===");
Console.WriteLine($"Видалено гру: {games.Delete(portal.Id)}");
Console.WriteLine($"Видалено жанр: {genres.Delete(puzzle.Id)}");
Console.WriteLine($"Видалено видавця: {publishers.Delete(valve.Id)}");

Console.WriteLine($"\nЗалишилось: ігор {games.GetAll().Count}, " +
                  $"жанрів {genres.GetAll().Count}, видавців {publishers.GetAll().Count}");

Console.ReadKey();