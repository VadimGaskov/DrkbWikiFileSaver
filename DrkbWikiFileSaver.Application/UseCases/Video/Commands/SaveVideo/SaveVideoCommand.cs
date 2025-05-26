using DrkbWikiFileSaver.Domain.Utils;
using MediatR;

namespace DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveVideo;

public class SaveVideoCommand : IRequest<Result<SaveVideoResultDto>>
{
    public SaveVideoCommand(string fileName, byte[] content, string mimeType)
    {
        FileName = fileName;
        Content = content;
        MimeType = mimeType;
    }
    public string FileName { get; private set; }
    public Byte[] Content { get; private set; }
    public string MimeType { get; private set; }
}