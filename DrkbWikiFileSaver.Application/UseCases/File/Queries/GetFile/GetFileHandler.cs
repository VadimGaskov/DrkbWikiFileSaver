using AutoMapper;
using DrkbWikiFileSaver.Application.Interfaces;
using DrkbWikiFileSaver.Application.Interfaces.Configurations;
using DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveFile;
using DrkbWikiFileSaver.Domain.Interfaces;
using DrkbWikiFileSaver.Domain.Utils;
using MediatR;

namespace DrkbWikiFileSaver.Application.UseCases.File.GetFile;

public class GetFileHandler : IRequestHandler<GetFileCommand, Result<List<GetFileResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GetFileHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<GetFileResponse>>> Handle(GetFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.RelatedId == Guid.Empty)
            {
                return Result<List<GetFileResponse>>.BadRequest("Такого Id не существует");
            }
            
            var file = await _unitOfWork.File.GetByIdRelatedAsync(request.RelatedId);
            var result = _mapper.Map<List<GetFileResponse>>(file);
            return Result<List<GetFileResponse>>.Success(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}