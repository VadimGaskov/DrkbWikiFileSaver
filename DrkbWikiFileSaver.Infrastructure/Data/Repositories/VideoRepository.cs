using DrkbWikiFileSaver.Application.Interfaces;
using DrkbWikiFileSaver.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrkbWikiFileSaver.Infrastructure.Data.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly DrkbWikiFileSaverContext _context;

    public VideoRepository(DrkbWikiFileSaverContext context)
    {
        _context = context;
    }

    public async Task<List<Video>> GetAllAsync(Guid id)
    {
        return await _context.Videos.ToListAsync();
    }

    public async Task<Video?> GetByIdAsync(Guid id)
    {
        return await _context.Videos.FirstOrDefaultAsync(x=>x.Id == id);
    }

    public async Task AddAsync(Video video)
    {
        await _context.Videos.AddAsync(video);
    }

    public async Task DeleteAsync(Video video)
    {
        _context.Videos.Remove(video);
    }

    public async Task UpdateAsync(Video entity)
    {
        _context.Videos.Update(entity);
    }
}