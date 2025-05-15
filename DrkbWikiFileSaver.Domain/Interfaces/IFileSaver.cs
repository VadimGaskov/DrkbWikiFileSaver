namespace DrkbWikiFileSaver.Domain.Interfaces;

public interface IFileSaver
{
    public Task SaveFile(string filePath, byte[] fileContent);
}