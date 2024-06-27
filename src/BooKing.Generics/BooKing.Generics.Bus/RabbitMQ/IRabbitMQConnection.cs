using RabbitMQ.Client;

namespace BooKing.Generics.Bus.RabbitMQ;
public interface IRabbitMQConnection : IDisposable
{
    bool IsConnected { get; }

    bool TryConnect();

    IModel CreateModel();    
}
