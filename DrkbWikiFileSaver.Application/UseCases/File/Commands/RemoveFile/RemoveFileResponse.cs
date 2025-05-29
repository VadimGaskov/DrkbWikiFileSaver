using System.Text.Json.Serialization;

namespace DrkbWikiFileSaver.Application.UseCases.File.RemoveFile;

public class RemoveFileResponse
{
    [JsonPropertyName("StatusCode")]
    public string StatusCode { get; set; }
}