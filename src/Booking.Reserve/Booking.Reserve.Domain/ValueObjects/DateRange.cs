namespace BooKing.Reserve.Domain.ValueObjects;
public record DateRange
{
    public DateRange()
    {

    }

    public DateTime Start { get; init; }
    public DateTime End { get; init; }
    
    public int lengthInDays => DateOnly.FromDateTime(End).DayNumber - DateOnly.FromDateTime(Start).DayNumber;

    public static DateRange Create(DateTime start, DateTime end)
    {
        if (start.Date > end.Date)
            throw new ApplicationException("End date precedes start date.");

        return new DateRange
        {
            Start = start.Date,
            End = end.Date
        };
    }
}