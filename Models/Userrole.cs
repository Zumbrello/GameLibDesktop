using System;
using System.Collections.Generic;

namespace PokemonsAPI.Models;

public partial class Userrole
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
