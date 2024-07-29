using BooKing.Generics.EventSourcing;

namespace BooKing.Generics.Outbox.Events;

public class NewUserEmailEvent : Event
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    public NewUserEmailEvent(Guid userId, string name, string email) : base(userId)
    {
        Name = name;
        Email = email;
        UserId = userId;
    }
}
