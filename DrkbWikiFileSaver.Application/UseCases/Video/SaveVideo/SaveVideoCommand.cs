using System.Windows.Input;
using DrkbWikiFileSaver.Domain.Utils;
using MediatR;

namespace DrkbWikiFileSaver.Application.UseCases;

public class SaveVideoCommand : IRequest<Result>
{
    public SaveVideoCommand(string fileName, byte[] content)
    {
        FileName = fileName;
        Content = content;
    }
    public string FileName { get; private set; }
    public Byte[] Content { get; private set; }
}