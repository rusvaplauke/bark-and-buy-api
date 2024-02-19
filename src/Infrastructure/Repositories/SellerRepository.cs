using Dapper;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repositories;

public class SellerRepository : ISellerRepository
{
    private readonly IDbConnection _connection;

    public SellerRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<string?> GetSellerNameAsync(int id)
    {
        string sql = "SELECT name FROM sellers WHERE id=@id;";

        return await _connection.QuerySingleOrDefaultAsync<string>(sql, new { id = id });
    }
}
