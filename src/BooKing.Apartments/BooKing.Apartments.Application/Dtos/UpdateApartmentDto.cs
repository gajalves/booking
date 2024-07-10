namespace BooKing.Apartments.Application.Dtos;

public class UpdateApartmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public AddressDto Address { get; set; }
    public decimal Price { get; set; }
    public decimal CleaningFee { get; set; }
    public List<Guid> Amenities { get; set; }
    public string Imagepath { get; set; }
}