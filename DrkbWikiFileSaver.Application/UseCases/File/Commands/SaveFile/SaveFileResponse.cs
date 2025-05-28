using System.Text.Json.Serialization;

namespace DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveFile;

public class SaveFileResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("savedFileUrl")]
    public string SavedFileUrl { get; set; }
}