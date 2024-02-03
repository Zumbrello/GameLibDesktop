namespace PokemonsAPI.Models;
public class UserTokens {
    public string AccessToken {
        get;
        set;
    }
    public string UserName {
        get;
        set;
    }
    
    public string RefreshToken {
        get;
        set;
    }
}