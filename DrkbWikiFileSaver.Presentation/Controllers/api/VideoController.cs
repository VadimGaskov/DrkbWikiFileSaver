using DrkbWikiFileSaver.Application.Interfaces;
using DrkbWikiFileSaver.Application.Interfaces.Configurations;
using DrkbWikiFileSaver.Application.UseCases;
using DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveFile;
using DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveVideo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DrkbWikiFileSaver.Controllers.api;

[Route("api/video")]
public class VideoController : Controller
{
    private readonly IMediator _mediator;
    public VideoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("save")]
    public async Task<IActionResult> Save(IFormFile video, CancellationToken cancellationToken)
    {
        if (video.Length == 0 || video == null)
        {
            return BadRequest("Файл не был загружен.");
        }
        
        using var memoryStream = new MemoryStream();
        await video.CopyToAsync(memoryStream);

        var byteArray = memoryStream.ToArray();
        
        var result = await _mediator.Send(new SaveVideoCommand(video.FileName, memoryStream, video.ContentType), cancellationToken);
        await memoryStream.DisposeAsync();

        if(result.IsSuccess)
            return Ok(result.Data);

        return StatusCode(result.StatusCode, result.Error);
        
    }
}