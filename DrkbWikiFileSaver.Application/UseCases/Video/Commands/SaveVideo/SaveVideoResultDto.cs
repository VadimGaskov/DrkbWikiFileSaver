using System.Text.Json.Serialization;

namespace DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveVideo;

public class SaveVideoResultDto
{
    [JsonPropertyName("savedVideoUrl")]
    public string SavedVideoUrl { get; set; }
}