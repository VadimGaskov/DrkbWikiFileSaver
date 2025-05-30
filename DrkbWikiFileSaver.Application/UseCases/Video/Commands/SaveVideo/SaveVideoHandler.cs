﻿using DrkbWikiFileSaver.Application.Interfaces;
using DrkbWikiFileSaver.Application.Interfaces.Configurations;
using DrkbWikiFileSaver.Domain.Interfaces;
using DrkbWikiFileSaver.Domain.Utils;
using MediatR;

namespace DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveVideo;

public class SaveVideoHandler : IRequestHandler<SaveVideoCommand, Result<SaveVideoResultDto>>
{
    private readonly IFileSaver _fileSaver;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVideoConfiguration _videoConfiguration;
    private readonly IObjectStorageService _objectStorageService;
    private readonly ISelectelStorageConfiguration _selectelConfig;
    public SaveVideoHandler(IFileSaver fileSaver, IUnitOfWork unitOfWork, IVideoConfiguration videoConfiguration, IObjectStorageService objectStorageService, ISelectelStorageConfiguration selectelConfig)
    {
        _fileSaver = fileSaver;
        _unitOfWork = unitOfWork;
        _videoConfiguration = videoConfiguration;
        _objectStorageService = objectStorageService;
        _selectelConfig = selectelConfig;
    }

    public async Task<Result<SaveVideoResultDto>> Handle(SaveVideoCommand request, CancellationToken cancellationToken)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var uploadDirectory = Path.Combine(currentDirectory, "Upload");
        var videoDirectory = Path.Combine(uploadDirectory, "Video");

        // Убедимся, что папки существуют
        Directory.CreateDirectory(videoDirectory);

        var videoPath = Path.Combine(videoDirectory, request.FileName);
        
        try
        {
            ///TODO Изменить
            try
            {
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
                
                
                
                var videoUpload = new Domain.Entities.Video()
                {
                    Title = nameFile,
                    Url = _videoConfiguration.Url + request.FileName, // например, базовый URL + имя файла
                    FilePath = videoPath,  // сохраняем локальный путь
                    MimeType = "video/mp4" // желательно определить корректный MIME ??? надо ли?
                };

                await _unitOfWork.Video.AddAsync(videoUpload);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<SaveVideoResultDto>.Success(new SaveVideoResultDto() { SavedVideoUrl = videoUpload.Url });
            }
            catch (Exception e)
            {
                return Result<SaveVideoResultDto>.ServerError("Не удалось сохранить файл");
            }
            
          
        }
        catch (Exception e)
        {
            return Result<SaveVideoResultDto>.ServerError("Не удалось сохранить файл");
        }
    }
    

}