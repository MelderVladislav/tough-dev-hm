namespace UberPopug.Domains.Auth.Utility.Collections;

public static class CollectionsExtensions
{
    public static bool HasValues<T>(this IEnumerable<T> source)
    {
        return source?.Any() ?? false;
    }

    public static bool Empty<T>(this IEnumerable<T> source)
    {
        return !source?.Any() ?? true;
    }
}