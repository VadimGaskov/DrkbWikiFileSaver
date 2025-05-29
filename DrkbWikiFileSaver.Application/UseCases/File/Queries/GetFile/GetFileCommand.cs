using DrkbWikiFileSaver.Application.Interfaces;
using DrkbWikiFileSaver.Application.Interfaces.Configurations;
using DrkbWikiFileSaver.Domain.Interfaces;
using DrkbWikiFileSaver.Domain.Utils;
using MediatR;

namespace DrkbWikiFileSaver.Application.UseCases.File.GetFile;

public class GetFileCommand : IRequest<Result<List<GetFileResponse>>>
{
    public GetFileCommand(Guid relatedId)
    {
        RelatedId = relatedId;
    }
    
    public Guid RelatedId { get; set; }
}