using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Quartz;

using SolutionTemplate.Infrastructure.EF.Outbox.Entities;
using SolutionTemplate.Infrastructure.EF.Outbox.Options;

namespace SolutionTemplate.Infrastructure.EF.Outbox.Background;

[DisallowConcurrentExecution]
internal sealed class OutboxMessageCleaner : IJob
{
    private readonly DbContext _dbContext;
    private readonly ILogger _logger;
    private readonly IOptions<OutboxOptions> _options;

    public OutboxMessageCleaner(DbContext dbContext, ILogger<OutboxMessageCleaner> logger, IOptions<OutboxOptions> options)
    {
        _dbContext = dbContext;
        _logger = logger;
        _options = options;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await _dbContext.Set<OutboxMessage>()
                .Where(m => m.ProcessedDateUtc != null &&
                            m.ProcessedDateUtc < DateTime.UtcNow.AddDays(-_options.Value.MessageRetentionInDays))
                .ExecuteDeleteAsync(context.CancellationToken);
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
    [LoggerMessage(1, LogLevel.Error, "Error cleaning up Outbox messages")]
    public static partial void OutboxMessageCleanupError(this ILogger logger, Exception exception);
}
