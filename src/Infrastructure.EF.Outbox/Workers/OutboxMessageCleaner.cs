using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Quartz;

using SolutionTemplate.Infrastructure.EF.Outbox.Entities;
using SolutionTemplate.Infrastructure.EF.Outbox.Options;

namespace SolutionTemplate.Infrastructure.EF.Outbox.Workers;

[DisallowConcurrentExecution]
internal sealed class OutboxMessageCleaner : IJob
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptionsMonitor<OutboxOptions> _options;

    public OutboxMessageCleaner(IServiceProvider serviceProvider, IOptionsMonitor<OutboxOptions> options, ILogger<OutboxMessageCleaner> logger)
    {
        _serviceProvider = serviceProvider;
        _options = options;
        _logger = logger;     
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var option = _options.CurrentValue;

            using var scope = _serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

            await dbContext.Set<OutboxMessage>()
                .Where(m => m.ProcessedDateUtc != null &&
                            m.ProcessedDateUtc < DateTime.UtcNow.AddDays(-option.MessageRetentionInDays))
                .ExecuteDeleteAsync(context.CancellationToken);

            _logger.OutboxMessagesCleaned(option.MessageRetentionInDays);
        }
        catch (Exception e)
        {
            _logger.OutboxMessageCleanupError(e);

            // We do not throw here, because we want the job to run again.
        }
    }
}

public static partial class Log
{
    [LoggerMessage(1, LogLevel.Critical, "Error cleaning up Outbox messages")]
    public static partial void OutboxMessageCleanupError(this ILogger logger, Exception exception);

    [LoggerMessage(2, LogLevel.Information, "Cleaned outbox messages older then {days} days")]
    public static partial void OutboxMessagesCleaned(this ILogger logger, int days);
}
