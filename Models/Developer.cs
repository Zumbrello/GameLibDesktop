using System;
using System.Collections.Generic;

namespace GameLibDesktop.Models;

public partial class Developer
{
    public int Id { get; set; }

    public string Developer1 { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
