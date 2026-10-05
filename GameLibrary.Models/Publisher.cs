namespace GameLibrary.Models;

public class Publisher
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int FoundedYear { get; set; }
    public string Website { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public List<Game> Games { get; set; } = new();
}