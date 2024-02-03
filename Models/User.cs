using System;
using System.Collections.Generic;

namespace PokemonsAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int UserRole { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<UserJwt> UserJwts { get; set; } = new List<UserJwt>();

    public virtual Userrole UserRoleNavigation { get; set; } = null!;
}
