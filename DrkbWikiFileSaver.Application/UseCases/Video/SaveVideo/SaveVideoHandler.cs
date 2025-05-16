using System.Net;
using DrkbWikiFileSaver.Application.Interfaces;
using DrkbWikiFileSaver.Application.Interfaces.Configurations;
using DrkbWikiFileSaver.Domain.Entities;
using DrkbWikiFileSaver.Domain.Interfaces;
using DrkbWikiFileSaver.Domain.Utils;
using MediatR;

namespace DrkbWikiFileSaver.Application.UseCases;

public class SaveVideoHandler : IRequestHandler<SaveVideoCommand, Result>
{
    private readonly IFileSaver _fileSaver;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVideoConfiguration _videoConfiguration;
    public SaveVideoHandler(IFileSaver fileSaver, IUnitOfWork unitOfWork, IVideoConfiguration videoConfiguration)
    {
        _fileSaver = fileSaver;
        _unitOfWork = unitOfWork;
        _videoConfiguration = videoConfiguration;
    }

    public async Task<Result> Handle(SaveVideoCommand request, CancellationToken cancellationToken)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var uploadDirectory = Path.Combine(currentDirectory, "Upload"); 
        var videoDirectory = Path.Combine(uploadDirectory, "Video"); 
        var videoPath = Path.Combine(videoDirectory, request.FileName);
        
        try
        {
            ///TODO Изменить
            try
            {
                await _fileSaver.SaveFile(videoPath, request.Content);
            }
            catch (Exception e)
            {
                
            }
            
            var video = new Video()
            {
                Title = request.FileName,
                Url = _videoConfiguration.Url + request.FileName,
            };

            await _unitOfWork.Video.AddAsync(video);

            await _unitOfWork.SaveChangesAsync();
            
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.ServerError("Не удалось сохранить файл");
        }
    }
}