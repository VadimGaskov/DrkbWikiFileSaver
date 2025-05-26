namespace DrkbWikiFileSaver.Application.Interfaces;

public interface IObjectStorageService
{
    public Task UploadFileAsync(string bucketName, string key, Stream fileStream);
}