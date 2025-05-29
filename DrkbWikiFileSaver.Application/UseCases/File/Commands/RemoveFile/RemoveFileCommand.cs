using DrkbWikiFileSaver.Domain.Utils;
using MediatR;

namespace DrkbWikiFileSaver.Application.UseCases.File.RemoveFile;

public class RemoveFileCommand : IRequest<Result<RemoveFileResponse>>
{
    public RemoveFileCommand(Guid idFile)
    {
        IdFile = idFile;
    }
    public Guid IdFile { get; set; } 
}