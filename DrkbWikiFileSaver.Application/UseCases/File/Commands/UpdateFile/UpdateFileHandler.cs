using AutoMapper;
using DrkbWikiFileSaver.Application.Interfaces;
using DrkbWikiFileSaver.Application.Interfaces.Configurations;
using DrkbWikiFileSaver.Domain.Interfaces;
using DrkbWikiFileSaver.Domain.Utils;
using MediatR;

namespace DrkbWikiFileSaver.Application.UseCases.File.Commands.UpdateFile;

public class UpdateFileHandler : IRequestHandler<UpdateFileCommand, Result<UpdateFileResponse>>
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IObjectStorageService _objectStorageService;
    private readonly ISelectelStorageConfiguration _selectelConfig;
    private readonly IMapper _mapper;
    public UpdateFileHandler(IUnitOfWork unitOfWork, IObjectStorageService objectStorageService, ISelectelStorageConfiguration selectelConfig, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _objectStorageService = objectStorageService;
        _selectelConfig = selectelConfig;
        _mapper = mapper;
    }
    
    public async Task<Result<UpdateFileResponse>> Handle(UpdateFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            try
            {
                //remove file, then upload file to selectel = updateFile
                
                //remove file from selectel
                var olderFile = await _unitOfWork.File.GetByIdAsync(request.IdFile);
                await _objectStorageService.RemoveFileAsync(_selectelConfig.BucketName, olderFile.Title);
                //upload file to selectel
                var formatFile = request.MimeType.SplitMimeType();
                var nameFile = Guid.NewGuid().ToString() + formatFile;
                string objectKey = nameFile;
                
                try
                {
                    await _objectStorageService.UploadFileAsync(_selectelConfig.BucketName, objectKey, request.Content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                
                
                
                /*var fileUpload = new Domain.Entities.File()
                {
                    Title = nameFile,
                    //TODO ПЕРЕДЕЛАТЬ НА НОРМАЛЬНЫЙ ПУТЬ
                    Url = "https://efed9ee2-8c7f-41ac-a003-3c21c6b97f6d.selstorage.ru" + "/" + nameFile, // например, базовый URL + имя файла
                    RelatedId = request.RelatedId,
                };*/

                olderFile.Title = nameFile;
                olderFile.Url = "https://efed9ee2-8c7f-41ac-a003-3c21c6b97f6d.selstorage.ru" + "/" + nameFile;

                await _unitOfWork.File.UpdateAsync(olderFile);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<UpdateFileResponse>.Success( _mapper.Map<UpdateFileResponse>(olderFile));
            }
            catch (Exception e)
            {
                return Result<UpdateFileResponse>.ServerError("Не удалось обновить файл");

            }
        }
        catch (Exception e)
        {
            return Result<UpdateFileResponse>.ServerError("Не удалось обновить файл");
        }
    }
}