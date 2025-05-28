using DrkbWikiFileSaver.Application.UseCases.File.GetFile;
using DrkbWikiFileSaver.Application.UseCases.File.RemoveFile;
using DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveFile;
using DrkbWikiFileSaver.Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DrkbWikiFileSaver.Controllers.api;
[Route("api/file")]
public class FileController : Controller
{
    private readonly IMediator _mediator;
    public FileController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("save")]
    public async Task<IActionResult> SaveFile(Guid idRelated, IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length == 0 || file == null)
        {
            return BadRequest("Файл не был загружен.");
        }
        
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        
        var result = await _mediator.Send(new SaveFileCommand(idRelated,file.FileName, memoryStream, file.ContentType), cancellationToken);
        await memoryStream.DisposeAsync();
        if(result.IsSuccess)
            return Ok(result.Data);

        return StatusCode(result.StatusCode, result.Error);
    }

    [HttpPost("remove")]
    public async Task<IActionResult> Remove(Guid idFile, CancellationToken cancellationToken)
    {
        if (idFile == Guid.Empty)
        {
            return BadRequest("Такого файла нет");
        }

        var result = await _mediator.Send(new RemoveFileCommand(idFile), cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return StatusCode(result.StatusCode, result.Error);
    }
    
    [HttpGet("get-files")]
    public async  Task<ActionResult<List<GetFileResponse>>> GetFiles(Guid idRelated, CancellationToken cancellationToken)
    {
        
        var result = await _mediator.Send(new GetFileCommand(idRelated), cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, result.Error);
        }
        return result.Data;
    }
    
}