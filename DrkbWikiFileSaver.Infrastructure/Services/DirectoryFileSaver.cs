using DrkbWikiFileSaver.Domain.Interfaces;

namespace DrkbWikiFileSaver.Infrastructure.Services;

public class DirectoryFileSaver : IFileSaver
{
    public async Task SaveFile(string filePath, byte[] fileContent)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllBytesAsync(filePath, fileContent);
    }
}