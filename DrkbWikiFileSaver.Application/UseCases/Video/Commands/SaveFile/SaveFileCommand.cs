using DrkbWikiFileSaver.Domain.Utils;
using MediatR;

namespace DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveFile;

public class SaveFileCommand: IRequest<Result<SaveFileResponse>>
{
    public SaveFileCommand(Guid relatedId, string title, MemoryStream content, string mimeType)
    {
        RelatedId = relatedId;
        Title = title;
        Content = content;
        MimeType = mimeType;
    }
    
    public Guid RelatedId { get; set; }
    public string Title { get; set; }
    public MemoryStream Content { get; set; }
    public string MimeType { get; set; }
}