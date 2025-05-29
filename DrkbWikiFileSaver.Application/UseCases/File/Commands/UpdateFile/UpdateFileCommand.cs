using DrkbWikiFileSaver.Domain.Utils;
using MediatR;

namespace DrkbWikiFileSaver.Application.UseCases.File.Commands.UpdateFile;

public class UpdateFileCommand : IRequest<Result<UpdateFileResponse>>
{

    public UpdateFileCommand(Guid idFile, MemoryStream content, string title, string mimeType)
    {
        IdFile = idFile;
        Content = content;
        Title = title;
        MimeType = mimeType;
    }
    public Guid IdFile { get; set; }
    public MemoryStream Content { get; set; }
    public string Title { get; set; }
    public string MimeType { get; set; }
}