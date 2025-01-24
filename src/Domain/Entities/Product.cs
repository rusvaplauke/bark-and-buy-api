namespace Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    // Nav
    public Guid CategoryId { get; set; } // cia pats EF supras, kad ID in lenta Category yra CategoryId
    public Category Category { get; set; }
}