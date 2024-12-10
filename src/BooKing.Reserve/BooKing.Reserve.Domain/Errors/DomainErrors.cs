using BooKing.Generics.Domain;

namespace BooKing.Reserve.Domain.Errors;
public static class DomainErrors
{
    public static class ReserveError
    {
        public static readonly Error EndDatePrecedesStartDate = new Error(
            "DateRange.Create",
            "End date precedes start date.");
    }
}
