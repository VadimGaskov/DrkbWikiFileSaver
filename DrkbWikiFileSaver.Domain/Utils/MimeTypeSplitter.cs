namespace DrkbWikiFileSaver.Domain.Utils;

public static class MimeTypeSplitter
{
    public static string SplitMimeType(this string str)
    {
        int slashIndex = str.IndexOf('/');
        if (slashIndex < 0 || slashIndex == str.Length - 1)
        {
            // Если нет символа '/' или он в конце строки, возвращаем пустую строку или саму строку по желанию
            return string.Empty;
        }
        return "." + str.Substring(slashIndex + 1);
    }
}