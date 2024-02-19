using Domain.Interfaces;
using Infrastructure.Clients;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, string? dbConnection)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(dbConnection));

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUserDataClient, JsonPlaceholderClient>();
    }
}
