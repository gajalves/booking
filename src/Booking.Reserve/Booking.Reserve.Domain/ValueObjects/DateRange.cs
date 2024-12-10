using BooKing.Generics.Domain;
using BooKing.Reserve.Domain.Errors;

namespace BooKing.Reserve.Domain.ValueObjects;
public record DateRange
{
    public DateRange()
    {

    }

    public DateTime Start { get; init; }
    public DateTime End { get; init; }
    
    public int lengthInDays => DateOnly.FromDateTime(End).DayNumber - DateOnly.FromDateTime(Start).DayNumber;

    public static Result<DateRange> Create(DateTime start, DateTime end)
    {
        if (start.Date > end.Date)
            return Result.Failure<DateRange>(DomainErrors.ReserveError.EndDatePrecedesStartDate);

        var duration = new DateRange
        {
            Start = start.Date,
            End = end.Date
        };

        return Result.Success(duration); 
    }
}