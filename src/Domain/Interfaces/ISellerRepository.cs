namespace Domain.Interfaces;

public interface ISellerRepository
{
    public Task<string?> GetSellerNameAsync(int id);
}
