public static class SlugHelper
{
    public static string Generate(string name)
    {
        return name
            .ToLower()
            .Trim()
            .Replace(" ", "-")
            .Replace("_", "-")
            .Replace("--", "-");
    }
}