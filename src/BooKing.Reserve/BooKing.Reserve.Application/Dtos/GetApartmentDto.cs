namespace BooKing.Reserve.Application.Dtos;
public class GetApartmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public AddressDto Address { get; set; }
    public decimal Price { get; set; }
    public decimal CleaningFee { get; set; }
    public List<AmenityDto> Amenities { get; set; }
    public string AmenitiesDescription { get => string.Join(", ", Amenities.Select(a => a.Name)); }
    public string ImagePath { get; set; }
    public string OwnerId { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
}

public class AddressDto
{
    public string Country { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
}

public class AmenityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}