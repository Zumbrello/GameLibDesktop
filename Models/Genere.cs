using System;
using System.Collections.Generic;

namespace GameLibDesktop.Models;

public partial class Genere
{
    public int Id { get; set; }

    public string Gener { get; set; } = null!;

    public virtual ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
}
