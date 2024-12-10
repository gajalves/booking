using Newtonsoft.Json;

namespace BooKing.Generics.Infra.Serialization;
public static class SerializerSettings
{
    public static readonly JsonSerializerSettings Instance = new()
    {
        TypeNameHandling = TypeNameHandling.All,
    };
}
