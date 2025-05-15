
namespace DrkbWikiFileSaver.Application.Interfaces;

public interface IUnitOfWork
{
    public IVideoRepository? Video { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}