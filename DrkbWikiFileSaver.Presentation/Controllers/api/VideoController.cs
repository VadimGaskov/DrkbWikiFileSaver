using DrkbWikiFileSaver.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace DrkbWikiFileSaver.Controllers.api;

[Route("api/video")]
public class VideoController : Controller
{
    private readonly SaveVideoHandler _videoHandler;

    public VideoController(SaveVideoHandler videoHandler)
    {
        _videoHandler = videoHandler;
    }

    public IActionResult GetAll()
    {
        return Ok();
    }

    [HttpPost("save")]
    public async Task<IActionResult> Save(IFormFile video)
    {
        if (video.Length == 0)
        {
            return BadRequest("Файл не был загружен.");
        }
        
        _videoHandler.Handle(new SaveVideoCommand());
        
        using var memoryStream = new MemoryStream();
        await video.CopyToAsync(memoryStream);
        
        return Ok();
    }
    
}