using System.Text.Json.Serialization;

namespace DrkbWikiFileSaver.Application.UseCases.File.GetFile;

public class GetFileResponse 
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("url")]
    public string Url { get; set; }
}