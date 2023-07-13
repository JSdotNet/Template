using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Quartz;

using SolutionTemplate.Infrastructure.EF.Outbox.Entities;

namespace SolutionTemplate.Infrastructure.EF.Outbox.Background;

[DisallowConcurrentExecution]
internal sealed class OutboxMessageCleaner : IJob
{
    private readonly DbContext _dbContext;
    private readonly ILogger _logger;

    public OutboxMessageCleaner(DbContext dbContext, ILogger<OutboxMessageCleaner> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await _dbContext.Set<OutboxMessage>()
                .Where(m => m.ProcessedDateUtc != null &&
                            m.ProcessedDateUtc < DateTime.UtcNow.AddDays(-10)) // TODO Make period configurable and simulate test case
                .ExecuteDeleteAsync(context.CancellationToken);
        }
        catch (Exception e)
        {
            _logger.OutboxMessageCleanupError(e);
        }
    }
}

public static partial class Log
{
    [LoggerMessage(1, LogLevel.Error, "Error cleaning up Outbox messages")]
    public static partial void OutboxMessageCleanupError(this ILogger logger, Exception exception);
}
