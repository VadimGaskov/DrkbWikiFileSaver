using DrkbWikiFileSaver.Application.Interfaces;
using DrkbWikiFileSaver.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;
using File = DrkbWikiFileSaver.Domain.Entities.File;

namespace DrkbWikiFileSaver.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DrkbWikiFileSaverContext _context;
    
    private VideoRepository? _videoRepository;
    private FileRepository? _fileRepository;
    
    public UnitOfWork(DrkbWikiFileSaverContext context, FileRepository? fileRepository)
    {
        _context = context;
        _fileRepository = fileRepository;
    }
    
    public IVideoRepository Video => _videoRepository ??= new VideoRepository(_context);
    public IFileRepository File => _fileRepository ??= new FileRepository(_context);
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}