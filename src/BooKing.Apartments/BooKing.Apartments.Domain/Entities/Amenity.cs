using BooKing.Generics.Domain;

namespace BooKing.Apartments.Domain.Entities;

public class Amenity : Entity
{
    public Amenity(string name)
    {
        Name = name;
    }

    public Amenity(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Name { get; private set; }
}