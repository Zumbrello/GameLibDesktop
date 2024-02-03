using System;
using System.Collections.Generic;

namespace PokemonsAPI.Models;

public partial class Game
{
    public int Id { get; set; }

    public string GameName { get; set; } = null!;

    public int IdDeveloper { get; set; }

    public int IdPublisher { get; set; }

    public string Description { get; set; } = null!;

    public string? SystemRequestMin { get; set; }

    public string? SystemRequestRec { get; set; }

    public string ReleaseDate { get; set; } = null!;

    public string MainImage { get; set; } = null!;

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();

    public virtual Developer IdDeveloperNavigation { get; set; } = null!;

    public virtual Publisher IdPublisherNavigation { get; set; } = null!;
}



