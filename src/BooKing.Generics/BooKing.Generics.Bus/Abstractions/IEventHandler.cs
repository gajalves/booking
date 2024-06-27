namespace BooKing.Generics.Bus.Abstractions;

public interface IEventHandler<TEvent> where TEvent : IEvent
{
    Task<bool> Handle(TEvent @event);
}
