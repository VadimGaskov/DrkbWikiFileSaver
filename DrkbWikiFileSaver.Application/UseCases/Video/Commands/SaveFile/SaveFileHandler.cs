using DrkbWikiFileSaver.Application.Interfaces;
using DrkbWikiFileSaver.Application.Interfaces.Configurations;
using DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveVideo;
using DrkbWikiFileSaver.Domain.Interfaces;
using DrkbWikiFileSaver.Domain.Utils;

namespace DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveFile;

public class SaveFileHandler
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
        var currentDirectory = Directory.GetCurrentDirectory();
        var uploadDirectory = Path.Combine(currentDirectory, "Upload");
        var fileDirectory = Path.Combine(uploadDirectory, "File");

        // Убедимся, что папки существуют
        Directory.CreateDirectory(fileDirectory);

        var videoPath = Path.Combine(fileDirectory, request.Title);
        
        try
        {
            ///TODO Изменить переделать чтобы селектел отпралялся из IFormFile
            try
            {
                // Открываем поток для загрузки в объектное хранилище
                await using var fileStream = new FileStream(videoPath, FileMode.Open, FileAccess.Read);
                
                //new name for file
                var formatFile = request.ContentType;
                var nameFile = Guid.NewGuid().ToString() + formatFile;
                // Задаём ключ объекта (например, имя файла)
                string objectKey = nameFile;

                // Загружаем файл в хранилище
                //await _objectStorageService.UploadFileAsync(_selectelConfig.BucketName, objectKey, fileStream);
                
                try
                {
                    await _objectStorageService.UploadFileAsync(_selectelConfig.BucketName, objectKey, fileStream);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                
                
                
                var fileUpload = new Domain.Entities.File()
                {
                    Title = nameFile,
                    //TODO ПЕРЕДЕЛАТЬ НА НОРМАЛЬНЫЙ ПУТЬ
                    Url = "https://efed9ee2-8c7f-41ac-a003-3c21c6b97f6d.selstorage.ru" + "/" + nameFile, // например, базовый URL + имя файла
                    RelatedId = request.RelatedId,
                };

                await _unitOfWork.File.AddAsync(fileUpload);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<SaveFileResponse>.Success(new SaveFileResponse() { SavedFileUrl = fileUpload.Url });
            }
            catch (Exception e)
            {
                
            }
            
            var file = new Domain.Entities.File()
            {
                Title = request.Title,
                Url = _videoConfiguration.Url + request.Title,
                RelatedId = request.RelatedId,
            };

            await _unitOfWork.File.AddAsync(file);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            //TODO подключить автомаппер
            return Result<SaveFileResponse>.Success(new SaveFileResponse() {SavedFileUrl = file.Url});
        }
        catch (Exception e)
        {
            return Result<SaveFileResponse>.ServerError("Не удалось сохранить файл");
        }
    }
    
    
}