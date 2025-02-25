namespace BooKing.Reserve.Application.Dtos;
public class ReservationEventsDto
{
    public Guid Id { get; set; }
    public string EventType { get; set; }
    public string EventTypeDescription { get; set; }
    public string Icon { get; set; }
    public DateTime CreatedAt { get; set; }
    public string AdditionalInformation { get; set; }
}
