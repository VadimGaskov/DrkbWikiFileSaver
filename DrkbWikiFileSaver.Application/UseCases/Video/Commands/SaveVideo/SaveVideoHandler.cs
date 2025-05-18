using DrkbWikiFileSaver.Application.Interfaces;
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
    public SaveVideoHandler(IFileSaver fileSaver, IUnitOfWork unitOfWork, IVideoConfiguration videoConfiguration)
    {
        _fileSaver = fileSaver;
        _unitOfWork = unitOfWork;
        _videoConfiguration = videoConfiguration;
    }

    public async Task<Result<SaveVideoResultDto>> Handle(SaveVideoCommand request, CancellationToken cancellationToken)
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
            
            var video = new Domain.Entities.Video()
            {
                Title = request.FileName,
                Url = _videoConfiguration.Url + request.FileName,
                FilePath = "_videoConfiguration.Path",
                MimeType = "asd"
            };

            await _unitOfWork.Video.AddAsync(video);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            //TODO подключить автомаппер
            return Result<SaveVideoResultDto>.Success(new SaveVideoResultDto() {SavedVideoUrl = video.Url});
        }
        catch (Exception e)
        {
            return Result<SaveVideoResultDto>.ServerError("Не удалось сохранить файл");
        }
    }
}