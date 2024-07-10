namespace BooKing.Apartments.Domain.ValueObjects;
public record Address(
    string Country,
    string State,
    string ZipCode,
    string City,
    string Street,
    string Number);
