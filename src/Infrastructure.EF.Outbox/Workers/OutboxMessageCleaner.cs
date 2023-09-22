using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SolutionTemplate.Infrastructure.EF.Outbox.Entities;
using SolutionTemplate.Infrastructure.EF.Outbox.Options;

namespace SolutionTemplate.Infrastructure.EF.Outbox.Workers;

internal sealed class OutboxMessageCleaner : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;
    private int _intervalInSeconds;

    public OutboxMessageCleaner(IServiceProvider serviceProvider, IOptions<OutboxOptions> options, ILogger<OutboxMessageCleaner> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;

        // We will reevaluate the options each run, but we need the interval te start with.
        _intervalInSeconds = options.Value.MessageProcessorIntervalInSeconds;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) 
        {
            await RunTimedLoop(stoppingToken);
        }
    }

    private async Task RunTimedLoop(CancellationToken stoppingToken)
    {
        try
        {
            using PeriodicTimer timer = new(TimeSpan.FromDays(_intervalInSeconds)); // TODO Support Cron schedule?

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                var intervalInSeconds = await ProcessScopedRun(stoppingToken);

                if (_intervalInSeconds != intervalInSeconds)
                {
                    _intervalInSeconds = intervalInSeconds;

                    // The options have changed, so we need to stop this job and let the host restart it.
                    timer.Dispose();
                    return;
                }
            }
        }
        catch (Exception e)
        {
            _logger.OutboxMessageCleanupError(e);

            // We do not throw here, because we want the job to run again.
        }
    }

    private async Task<int> ProcessScopedRun(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var optionsSnapshot = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<OutboxOptions>>();
        var options = optionsSnapshot.Value;

        await CleanupMessages(scope, options, stoppingToken);

        return options.MessageCleanupIntervalDays;
    }


    private async Task CleanupMessages(IServiceScope scope, OutboxOptions options, CancellationToken stoppingToken)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

        await dbContext.Set<OutboxMessage>()
            .Where(m =>
                m.ProcessedDateUtc != null &&
                m.ProcessedDateUtc < DateTime.UtcNow.AddDays(-options.MessageRetentionInDays))
            .ExecuteDeleteAsync(stoppingToken);

        _logger.OutboxMessagesCleaned(options.MessageRetentionInDays);
    }
}

public static partial class Log
{
    [LoggerMessage(1, LogLevel.Critical, "Error cleaning up Outbox messages")]
    public static partial void OutboxMessageCleanupError(this ILogger logger, Exception exception);

    [LoggerMessage(2, LogLevel.Information, "Cleaned outbox messages older then {days} days")]
    public static partial void OutboxMessagesCleaned(this ILogger logger, int days);
}
