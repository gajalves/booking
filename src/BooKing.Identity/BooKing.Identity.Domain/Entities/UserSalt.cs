using BooKing.Generics.Domain;

namespace BooKing.Identity.Domain.Entities;
public class UserSalt : Entity
{    
    public Guid UserId { get; private set; }
    public string Salt { get; private set; }

    public virtual User User { get; set; }

    public UserSalt() { }

    public UserSalt(Guid userId, string salt)
    {        
        UserId = userId;
        Salt = salt;
    }
}