namespace BooKing.Reserve.Application.Dtos;
public class NewReservationDto
{    
    public Guid ApartmentId { get; set; }    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }    
}
