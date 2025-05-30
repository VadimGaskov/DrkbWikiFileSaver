using DrkbWikiFileSaver.Domain.Utils;
using MediatR;

namespace DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveFile;

public class SaveFileCommand: IRequest<Result<SaveFileResponse>>
{
    public SaveFileCommand(Guid relatedId, string fileTitle, MemoryStream content, string mimeType, string? requestTitle)
    {
        RelatedId = relatedId;
        FileTitle = fileTitle;
        Content = content;
        MimeType = mimeType;
        RequestTitle = requestTitle;
    }
    
    public Guid RelatedId { get; set; }
    public string? RequestTitle { get; set; }
    public string FileTitle { get; set; }
    public MemoryStream Content { get; set; }
    public string MimeType { get; set; }
}