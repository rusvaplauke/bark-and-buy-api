using Dapper;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repositories;

public class StatusRepository : IStatusRepository
{
    private readonly IDbConnection _connection;

    public StatusRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<string?> GetStatusValueAsync(int id)
    {
        string sql = "SELECT status FROM statuses WHERE id=@id;";

        return await _connection.QuerySingleOrDefaultAsync<string>(sql, new { id = id });
    }
}
