namespace Domain.Helpers;

public static class StatusHelper
{
    public static string SetStatus(DateTime? start, DateTime? end)
    {
        var currentDate = DateTime.Now;

        if (currentDate < start)
            return "Pending";

        if (currentDate >= start && currentDate <= end)
            return "Started";

        else
            return "Completed";
    }
}
