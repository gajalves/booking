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
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }
    public string SearchField { get ; private set; }    

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
        IsActive = true;
        IsDeleted = false;
    }
    
    public void AddAmenitie(Amenity amenitie)
    {
        if(Amenities is null)
            Amenities = new();

        Amenities.Add(amenitie);
    }

    public void Update(string name, string description, Address address, decimal price, decimal cleaningFee, string imagePath)
    {
        Name = name;
        Description = description;
        Address = address;
        Price = price;
        CleaningFee = cleaningFee;
        ImagePath = imagePath;        
    }

    public void SetSearchField()
    {
        SearchField = $"{Name}-{Description}-{Address.City}-{Address.State}-{string.Join("-", Amenities.Select(a => a.Name))}";
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

    public void SetIsActive(bool isActive)
    {
        IsActive = isActive;
    }

    public void SetIsDeleted(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }
}
