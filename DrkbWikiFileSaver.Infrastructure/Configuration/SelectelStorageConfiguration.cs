using DrkbWikiFileSaver.Application.Interfaces.Configurations;

namespace DrkbWikiFileSaver.Infrastructure.Configuration;

public class SelectelStorageConfiguration : ISelectelStorageConfiguration
{
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string ServiceUrl { get; set; }
    public string BucketName { get; set; }
    public string Region { get; set; }
}