using System.Net;
using DrkbWikiFileSaver.Application.Interfaces;
using DrkbWikiFileSaver.Domain.Utils;
using MediatR;

namespace DrkbWikiFileSaver.Application.UseCases.File.RemoveFile;

public class RemoveFileHandler : IRequestHandler<RemoveFileCommand, Result<RemoveFileResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveFileHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RemoveFileResponse>> Handle(RemoveFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var file = await _unitOfWork.File.GetByIdAsync(request.IdFile);
            await _unitOfWork.File.DeleteAsync(file);
            await _unitOfWork.SaveChangesAsync();
            return Result<RemoveFileResponse>.Success(new RemoveFileResponse()
                { StatusCode = HttpStatusCode.OK.ToString() });
        }
        catch (Exception e)
        {
            return Result<RemoveFileResponse>.ServerError("Не удалось удалить файл");
        }
    }
}