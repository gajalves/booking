namespace BooKing.Generics.Shared.CurrentUserService;
public class CurrentUser
{
    public CurrentUser(Guid id, string email, string token)
    {
        Id = id;
        Email = email;
        Token = token;
    }

    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Token { get; private set;}
}
