using DrkbWikiFileSaver.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using File = DrkbWikiFileSaver.Domain.Entities.File;

namespace DrkbWikiFileSaver.Infrastructure.Data;

public class DrkbWikiFileSaverContext : DbContext
{
    public DrkbWikiFileSaverContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<Video> Videos { get; set; }
    public DbSet<File> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}