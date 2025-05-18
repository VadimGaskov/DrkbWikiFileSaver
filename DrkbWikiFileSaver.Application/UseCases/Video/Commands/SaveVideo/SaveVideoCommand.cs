using DrkbWikiFileSaver.Domain.Utils;
using MediatR;

namespace DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveVideo;

public class SaveVideoCommand : IRequest<Result<SaveVideoResultDto>>
{
    public SaveVideoCommand(string fileName, byte[] content)
    {
        FileName = fileName;
        Content = content;
    }
    public string FileName { get; private set; }
    public Byte[] Content { get; private set; }
}