namespace DrkbWikiFileSaver.Domain.Entities;

public class File : BaseClass
{
    public Guid RelatedId { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    
}