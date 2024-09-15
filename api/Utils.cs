namespace StaMemory;

public static class Utils
{
    public static string IssueId()
    {
        return Guid.NewGuid().ToString("N");
    }
}
