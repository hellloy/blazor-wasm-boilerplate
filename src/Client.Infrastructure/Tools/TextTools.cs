namespace FSH.BlazorWebAssembly.Client.Infrastructure.Tools;
public class TextTools
{
    public static string? TruncateText(string? text, int maxLength)
    {
        if (text == null || text.Length <= maxLength)
            return text;

        return text.Substring(0, maxLength) + "...";
    }
}
