namespace Domain.Interfaces;

public interface IStatusRepository
{
    public Task<string?> GetStatusValueAsync(int id);
}
