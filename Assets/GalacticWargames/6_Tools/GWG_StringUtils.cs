using System.Text.RegularExpressions;

public static class StringUtils
{
    public static string AddSpaces(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return Regex.Replace(
            text,
            "(?<=[a-z])(?=[A-Z])|(?<=[0-9])(?=[A-Z])",
            " "
        );
    }
}