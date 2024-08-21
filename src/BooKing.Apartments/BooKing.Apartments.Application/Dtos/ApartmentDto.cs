namespace BooKing.Apartments.Application.Dtos;
public class ApartmentDto
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
    public string OwnerId { get; private set; }
}
