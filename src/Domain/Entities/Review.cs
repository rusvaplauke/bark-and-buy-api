namespace Domain.Entities;

public class Review : BaseEntity
{
    public string Content { get; set; }
    
    // Nav
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}