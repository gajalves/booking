using BooKing.Generics.Domain;

namespace BooKing.Identity.Domain.Entities;
public class User : Entity
{    
    public string Email { get; private set; }
    public string Name { get; private set; }
    public string Password { get; private set; }

    public virtual UserSalt UserSalt { get; set; }

    public User() { }

    public User(string email, string name, string password, string salt)
    {        
        Email = email;
        Name = name;
        Password = password;

        UserSalt = new UserSalt(Id, salt);
    }
}
