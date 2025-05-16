using DrkbWikiFileSaver.Application.Interfaces.Configurations;
using Microsoft.Extensions.Options;

namespace DrkbWikiFileSaver.Infrastructure.Configuration;

public class VideoConfiguration : IVideoConfiguration
{
    private readonly IOptionsMonitor<VideoSettings> _videoSettings;

    public VideoConfiguration(IOptionsMonitor<VideoSettings> videoSettings)
    {
        _videoSettings = videoSettings;
    }

    public string Path => _videoSettings.CurrentValue.Path;
    public string Url => _videoSettings.CurrentValue.Url;
}