namespace Domain.Dtos
{
    public record Order(int id, string? status, string? seller, string? userName, string orderedAt); // TODO: Dates must be passed as strings encoded according to ISO-8601. 
}
