using System.Text.Json.Serialization;

namespace DrkbWikiFileSaver.Application.UseCases.File.Commands.UpdateFile;

public class UpdateFileResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
}