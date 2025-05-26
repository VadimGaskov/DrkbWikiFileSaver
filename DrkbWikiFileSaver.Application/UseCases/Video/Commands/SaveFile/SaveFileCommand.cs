using DrkbWikiFileSaver.Domain.Utils;
using MediatR;

namespace DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveFile;

public class SaveFileCommand: IRequest<Result<SaveFileResponse>>
{
    public SaveFileCommand(Guid relatedId, string title, byte[] content, string mimeType, string contentType)
    {
        RelatedId = relatedId;
        Title = title;
        Content = content;
        MimeType = mimeType;
        ContentType = contentType;
    }
    
    public Guid RelatedId { get; set; }
    public string Title { get; set; }
    public byte[] Content { get; set; }
    public string MimeType { get; set; }
    public string ContentType { get; set; }
}