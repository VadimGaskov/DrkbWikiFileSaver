using DrkbWikiFileSaver.Application.Interfaces;
using DrkbWikiFileSaver.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DrkbWikiFileSaver.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DrkbWikiFileSaverContext _context;
    
    private VideoRepository? _videoRepository;
    
    public UnitOfWork(DrkbWikiFileSaverContext context)
    {
        _context = context;
    }
    
    public IVideoRepository Video => _videoRepository ??= new VideoRepository(_context);
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}