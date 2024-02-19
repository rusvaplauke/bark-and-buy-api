namespace Domain.Dtos
{
    public record Order(int id, string? status, string? seller, string? userName, string orderedAt);
}
