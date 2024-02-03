using System;
using System.Collections.Generic;

namespace PokemonsAPI.Models;

public partial class Publisher
{
    public int Id { get; set; }

    public string Publisher1 { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
