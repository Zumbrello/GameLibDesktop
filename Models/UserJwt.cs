using System;
using System.Collections.Generic;

namespace GameLibDesktop.Models;

public partial class UserJwt
{
    public int Id { get; set; }

    public int User { get; set; }

    public string? JwtRefresh { get; set; }

    public string? JwtAccess { get; set; }

    public virtual User UserNavigation { get; set; } = null!;
}
