namespace DrkbWikiFileSaver.Application.Interfaces.Configurations;

public interface ISelectelStorageConfiguration
{
    string AccessKey { get; }
    string SecretKey { get; }
    string ServiceUrl { get; }
    string BucketName { get; }
    string Region { get; }
}