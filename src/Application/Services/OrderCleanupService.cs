using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Services;

public class OrderCleanupService : BackgroundService
{
    private readonly TimeSpan _period;
    private readonly IServiceScopeFactory _orderServiceScopeFactory;

    public OrderCleanupService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
    {
        _orderServiceScopeFactory = serviceScopeFactory;

        double cleanupPeriod;
        if (!double.TryParse(configuration["PeriodicCleanup"], out cleanupPeriod))
        {
            throw new ArgumentNullException("PeriodicCleanup");
        }
        _period = TimeSpan.FromMinutes(cleanupPeriod);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_period);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using (var scope = _orderServiceScopeFactory.CreateScope())
                {
                    var scopedService = scope.ServiceProvider.GetRequiredService<OrderService>();
                    await scopedService.CleanUpExpired();
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
    }
}
