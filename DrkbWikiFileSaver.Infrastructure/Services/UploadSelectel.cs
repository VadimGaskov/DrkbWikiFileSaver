using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using DrkbWikiFileSaver.Application.Interfaces;
using DrkbWikiFileSaver.Application.Interfaces.Configurations;

namespace DrkbWikiFileSaver.Infrastructure.Services;

public class UploadSelectel : IObjectStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly ISelectelStorageConfiguration _config;

    public UploadSelectel(ISelectelStorageConfiguration config)
    {
        _config = config;

        var credentials = new BasicAWSCredentials(_config.AccessKey, _config.SecretKey);
        var s3Config = new AmazonS3Config
        {
            ServiceURL = _config.ServiceUrl,
            ForcePathStyle = true
        };

        _s3Client = new AmazonS3Client(credentials, s3Config);
    }

    public async Task UploadFileAsync(string bucketName, string key, Stream fileStream)
    {
        var fileTransferUtility = new TransferUtility(_s3Client);

        await fileTransferUtility.UploadAsync(fileStream, bucketName, key);
    }
}