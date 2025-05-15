using DrkbWikiFileSaver.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DrkbWikiFileSaver.Infrastructure.Data;

public class DrkbWikiFileSaverContext : DbContext
{
    public DrkbWikiFileSaverContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<Video> Videos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}