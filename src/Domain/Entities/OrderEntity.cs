namespace Domain.Entities;

public class OrderEntity
{
    public int Id { get; set; } = 0;
    public int StatusId { get; set; } = 1;
    public int SellerId { get; set; } = 0;
    public int UserId { get; set; } = 0;
    public DateTime OrderedAt { get; set; } = DateTime.Now;
}
