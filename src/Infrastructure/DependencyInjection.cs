using Domain.Interfaces;
using Infrastructure.Clients;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, string? dbConnection, string? dbConnectionForEFC)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddHttpClient();

        services.AddTransient<IDbConnection>(sp => new NpgsqlConnection(dbConnection));
        
        services.AddDbContext<MyDbContext>(options => options.UseNpgsql(dbConnectionForEFC));

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
        services.AddScoped<ISellerRepository, SellerRepository>();
        services.AddScoped<IUserDataClient, JsonPlaceholderClient>();
    }
}
