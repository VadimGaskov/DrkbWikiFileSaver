using DrkbWikiFileSaver.Application.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DrkbWikiFileSaver.Controllers.api;

[Route("api/video")]
public class VideoController : Controller
{
    private readonly SaveVideoHandler _videoHandler;
    private readonly IMediator _mediator;
    public VideoController(SaveVideoHandler videoHandler, IMediator mediator)
    {
        _videoHandler = videoHandler;
        _mediator = mediator;
    }

    [HttpPost("save")]
    public async Task<IActionResult> Save(IFormFile video)
    {
        if (video.Length == 0)
        {
            return BadRequest("Файл не был загружен.");
        }

        using var memoryStream = new MemoryStream();
        await video.CopyToAsync(memoryStream);
        
        var result = await _mediator.Send(new SaveVideoCommand(video.FileName, memoryStream.ToArray()));
        
        return Ok();
    }
    
}