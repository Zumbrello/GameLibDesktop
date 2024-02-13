using System;
using System.Collections.Generic;

namespace GameLibDesktop.Models;

public partial class Favorite
{
    public int IdUser { get; set; }

    public int IdGame { get; set; }

    public int Id { get; set; }

    public virtual Game IdGameNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
