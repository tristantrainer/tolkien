namespace KiriathSolutions.Tolkien.Api.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetStartOfDay(this DateTime current)
        {
            return new DateTime(current.Year, current.Month, current.Day);
        }
    }
}