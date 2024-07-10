using BooKing.Apartments.Domain.ValueObjects;
using BooKing.Generics.Domain;

namespace BooKing.Apartments.Domain.Entities;
public class Apartment : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Address Address { get; private set; }
    public decimal Price { get; private set; }
    public decimal CleaningFee { get; private set; }
    public DateTime? LastBookedOnUtc { get; internal set; }
    public List<Amenity> Amenities { get; private set; }
    public string OwnerId { get; private set; }
    public string ImagePath { get; private set; }

    public Apartment()
    {        
    }

    public Apartment(string name, 
                     string description, 
                     Address address, 
                     decimal price, 
                     decimal cleaningFee,
                     string ownerId,
                     string imagepath)
    {
        Name = name;
        Description = description;
        Address = address;
        Price = price;
        CleaningFee = cleaningFee;
        OwnerId = ownerId;
        ImagePath = imagepath;
    }

    public Apartment(Guid id,
                     string name,
                     string description,
                     Address address,
                     decimal price,
                     decimal cleaningFee,
                     string ownerId,
                     string imagepath)
    {
        Id = id;
        Name = name;
        Description = description;
        Address = address;
        Price = price;
        CleaningFee = cleaningFee;
        OwnerId = ownerId;
        ImagePath = imagepath;
    }

    public void AddAmenitie(Amenity amenitie)
    {
        if(Amenities is null)
            Amenities = new();

        Amenities.Add(amenitie);
    }

    public void Update(string name, string description, Address address, decimal price, decimal cleaningFee)
    {
        Name = name;
        Description = description;
        Address = address;
        Price = price;
        CleaningFee = cleaningFee;
    }

    public void SetAmenities(List<Amenity> amenities)
    {
        if (Amenities is null)
            Amenities = new();

        Amenities = amenities;
    }

    public void SetImagePath(string imagePath)
    {
        ImagePath = imagePath;
    }
}
