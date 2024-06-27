namespace BooKing.Generics.Outbox.Events;
public class NewUserEmailEvent : Event
{
    public string Name { get; set; }
    public string Email { get; set; }

    public NewUserEmailEvent()
    {

    }

    public NewUserEmailEvent(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
