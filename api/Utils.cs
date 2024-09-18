namespace StaMemory;

public static class Utils
{
    public static string IssueId()
    {
        return Guid.NewGuid().ToString("N");
    }

    public static int GetClearTime(DateTime from, DateTime to)
    {
        return (int)Math.Ceiling((to - from).TotalSeconds);
    }
}
