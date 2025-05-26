using DrkbWikiFileSaver.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = DrkbWikiFileSaver.Domain.Entities.File;

namespace DrkbWikiFileSaver.Infrastructure.Data.Repositories;

public class FileRepository : IFileRepository
{
    
    private readonly DrkbWikiFileSaverContext _context;

    
    public FileRepository(DrkbWikiFileSaverContext context)
    {
        _context = context;
    }
    
    public async Task<List<File>> GetAllAsync(Guid id)
    {
        return await _context.Files.ToListAsync();
    }

    public async Task<File?> GetByIdAsync(Guid id)
    {
        return await _context.Files.FirstOrDefaultAsync(x=>x.Id == id);
    }

    public async Task AddAsync(File file)
    {
        await _context.Files.AddAsync(file);
    }

    public async Task DeleteAsync(File file)
    {
        _context.Files.Remove(file);
    }

    public async Task UpdateAsync(File file)
    {
        _context.Files.Update(file);
    }
}