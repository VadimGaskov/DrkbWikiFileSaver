namespace DrkbWikiFileSaver.Application.Interfaces;

public interface IFileRepository : IRepository<Domain.Entities.File>
{
    public Task<List<Domain.Entities.File>> GetByIdRelatedAsync(Guid idRelated);
        
}