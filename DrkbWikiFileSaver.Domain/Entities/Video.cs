namespace DrkbWikiFileSaver.Domain.Entities;

public class Video : BaseClass
{
    public string Title { get; set; }
    public string Url { get; set; }
    public string FilePath { get; set; }
    public string MimeType { get; set; }
    public DateTime UploadedAt { get; set; }
}