namespace GameLibrary.Models;

public class Game
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int ReleaseYear { get; set; }
    public double Rating { get; set; }
    public string Description { get; set; } = string.Empty;

    public int PublisherId { get; set; }
    public Publisher Publisher { get; set; } = null!;

    public List<Genre> Genres { get; set; } = new();
}