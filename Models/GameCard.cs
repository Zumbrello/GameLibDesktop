namespace PokemonsAPI.Models;

public class GameCard
{
    public int Id { get; set; }

    public string GameName { get; set; } = null!;

    public string Developer { get; set; }

    public string Publisher { get; set; }

    public string Description { get; set; } = null!;

    public string? SystemRequestMin { get; set; }

    public string? SystemRequestRec { get; set; }

    public string ReleaseDate { get; set; } = null!;

    public Avalonia.Media.Imaging.Bitmap MainImage { get; set; } = null!;
}