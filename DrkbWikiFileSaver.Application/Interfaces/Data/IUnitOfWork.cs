
namespace DrkbWikiFileSaver.Application.Interfaces;

public interface IUnitOfWork
{
    public IVideoRepository? Video { get; }
    public IFileRepository? File { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}