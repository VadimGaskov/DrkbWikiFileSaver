using DrkbWikiFileSaver.Application.Interfaces;
using DrkbWikiFileSaver.Application.Interfaces.Configurations;
using DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveVideo;
using DrkbWikiFileSaver.Domain.Interfaces;
using DrkbWikiFileSaver.Domain.Utils;
using MediatR;

namespace DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveFile;

public class SaveFileHandler : IRequestHandler<SaveFileCommand, Result<SaveFileResponse>>
{
    private readonly IFileSaver _fileSaver;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVideoConfiguration _videoConfiguration;
    private readonly IObjectStorageService _objectStorageService;
    private readonly ISelectelStorageConfiguration _selectelConfig;
    public SaveFileHandler(IFileSaver fileSaver, IUnitOfWork unitOfWork, IVideoConfiguration videoConfiguration, IObjectStorageService objectStorageService, ISelectelStorageConfiguration selectelConfig)
    {
        _fileSaver = fileSaver;
        _unitOfWork = unitOfWork;
        _videoConfiguration = videoConfiguration;
        _objectStorageService = objectStorageService;
        _selectelConfig = selectelConfig;
    }
    
    
     public async Task<Result<SaveFileResponse>> Handle(SaveFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            ///TODO Изменить переделать чтобы селектел отпралялся из IFormFile
            try
            {
                var formatFile = request.MimeType.SplitMimeType();
                var nameFile = Guid.NewGuid() + formatFile;
                string objectKey = nameFile;
                
                try
                {
                    await _objectStorageService.UploadFileAsync(_selectelConfig.BucketName, objectKey, request.Content);
                }
                catch (Exception ex)
                {
                    return Result<SaveFileResponse>.ServerError("Не удалось сохранить файл");
                }
                
                var fileUpload = new Domain.Entities.File()
                {
                    Title = request.RequestTitle ?? request.FileTitle,
                    //TODO ПЕРЕДЕЛАТЬ НА НОРМАЛЬНЫЙ ПУТЬ
                    Url = "https://efed9ee2-8c7f-41ac-a003-3c21c6b97f6d.selstorage.ru" + "/" + nameFile, // например, базовый URL + имя файла
                    RelatedId = request.RelatedId,
                };

                await _unitOfWork.File.AddAsync(fileUpload);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<SaveFileResponse>.Success(new SaveFileResponse() { SavedFileUrl = fileUpload.Url, Title = fileUpload.Title, Id = fileUpload.Id.ToString()});
            }
            catch (Exception e)
            {
                return Result<SaveFileResponse>.ServerError("Не удалось сохранить файл");
            }
        }
        catch (Exception e)
        {
            return Result<SaveFileResponse>.ServerError("Не удалось сохранить файл");
        }
    }
    
    
}