namespace BooKing.Generics.Shared.CurrentUserService;
public class CurrentUser
{
    public CurrentUser(Guid id, string email)
    {
        Id = id;
        Email = email;
    }

    public Guid Id { get; private set; }
    public string Email { get; private set; }
}
