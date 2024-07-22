namespace BooKing.Generics.Shared;
public static class DateTimeHelper
{
    public static DateTime HoraBrasilia() => TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));    
}
