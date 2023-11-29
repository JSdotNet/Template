using System.Diagnostics;

using SolutionTemplate.Application.Authors.Commands;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using SolutionTemplate.Domain;
using SolutionTemplate.Infrastructure.EF.Data;
using SolutionTemplate.Infrastructure.EF.Outbox.Entities;

using MediatR;

using SolutionTemplate.Integration.Tests.Helpers;


namespace SolutionTemplate.Integration.Tests;

[CollectionDefinition(nameof(OutboxTests), DisableParallelization = true)]
public sealed class OutboxTestCollectionFixture : ICollectionFixture<IntegrationTestApplicationFactory>;

[Collection(nameof(OutboxTests))]
public sealed class OutboxTests : IAsyncLifetime
{
    private readonly IntegrationTestApplicationFactory _factory;
    private readonly ITestOutputHelper _output;
    private readonly Func<Task> _resetDatabase;
    private readonly ISender _sender;
    private readonly IPublisher _publisher;

    public OutboxTests(IntegrationTestApplicationFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
        _resetDatabase = factory.ResetStateAsync;

        var scope = factory.Services.CreateScope();
        _sender = scope.ServiceProvider.GetRequiredService<ISender>();
        _publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
    }


    [Fact]
    public async Task TestOutbox()
    {
        // Arrange
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        var fixture = new Fixture();

        var command = new CreateAuthor.Command(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
        
        _output.WriteLine($"Preparing command took {stopWatch.ElapsedMilliseconds} ms");
        stopWatch.Restart();

        // Act
        var result = await _sender.Send(command);

        _output.WriteLine($"Sending command took {stopWatch.ElapsedMilliseconds} ms");
        stopWatch.Restart();

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Check consumers...
        await WaitForConsumer(result);

        _output.WriteLine($"Processing consumer took {stopWatch.ElapsedMilliseconds} ms");
        stopWatch.Restart();

        await WaitUntilDomainEventsHandled();

        _output.WriteLine($"Verifying database record took {stopWatch.ElapsedMilliseconds} ms");
        stopWatch.Stop();
    }


    [Fact]
    public async Task TestPublisher()
    {
        // Arrange
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        var fixture = new Fixture();

        var domainEvent = new DomainEvents.AuthorCreated(fixture.Create<Guid>());

        _output.WriteLine($"Preparing event took {stopWatch.ElapsedMilliseconds} ms");
        stopWatch.Restart();

        // Act
        await _publisher.Publish(domainEvent);

        _output.WriteLine($"Sending event took {stopWatch.ElapsedMilliseconds} ms");
        stopWatch.Restart();

        // Assert

        // - Check consumers...
        await WaitForConsumer(domainEvent.AuthorId);

        _output.WriteLine($"Processing consumer took {stopWatch.ElapsedMilliseconds} ms");
        stopWatch.Restart();

        //await WaitUntilDomainEventsHandled();

        //_output.WriteLine($"Verifying database record took {stopWatch.ElapsedMilliseconds} ms");
        //stopWatch.Stop();
    }


    [Fact]
    public async Task OutboxLoadTest()
    {
        // Arrange
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        var fixture = new Fixture();

        var commands = new List<CreateAuthor.Command>();
        for (int i = 0; i < 50; i++)
        {
            commands.Add(new CreateAuthor.Command(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>()));
        }

        var tasks = commands.Select(async command =>
        {
            var scope = _factory.Services.CreateScope();

            var sender = scope.ServiceProvider.GetRequiredService<ISender>();

            // Act
            var result = await sender.Send(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            return result.Value;
        }).ToList();

        _output.WriteLine($"Preparing {tasks.Count} commands took {stopWatch.ElapsedMilliseconds} ms");
        stopWatch.Restart();

        // Act
        var results = await Task.WhenAll(tasks);

        _output.WriteLine($"Sending {tasks.Count} commands took {stopWatch.ElapsedMilliseconds} ms");
        stopWatch.Restart();

        await WaitForConsumers(results);

        _output.WriteLine($"Processing {tasks.Count} consumers took {stopWatch.ElapsedMilliseconds} ms");
        stopWatch.Restart();

        await WaitUntilDomainEventsHandled();

        _output.WriteLine($"Verifying database record took {stopWatch.ElapsedMilliseconds} ms");
        stopWatch.Stop();
    }


    private async Task WaitForConsumers(Guid[] results, int timeoutInSeconds = 30, int delayBetweenAttemptsInMilliseconds = 200)
    {
        var logger = _factory.Services.GetRequiredService<IDomainEventLogger>();
        var start = DateTime.UtcNow;

        while (DateTime.UtcNow.Subtract(start).TotalSeconds < timeoutInSeconds)
        {
            if(results.All(r => logger.Get<DomainEvents.AuthorCreated>().Any(de => de.AuthorId == r)))
               return;

            await Task.Delay(delayBetweenAttemptsInMilliseconds);
        }
    }

    private async Task WaitForConsumer(Guid authorId, int timeoutInSeconds = 20, int delayBetweenAttemptsInMilliseconds = 200)
    {
        var start = DateTime.UtcNow;

        while (DateTime.UtcNow.Subtract(start).TotalSeconds < timeoutInSeconds)
        {
            var logger = _factory.Services.GetRequiredService<IDomainEventLogger>();

            if (logger.Get<DomainEvents.AuthorCreated>().Any(de => de.AuthorId == authorId))
                return;

            await Task.Delay(delayBetweenAttemptsInMilliseconds);
        }
    }

    private async Task WaitUntilDomainEventsHandled(int timeoutInSeconds = 20, int delayBetweenAttemptsInMilliseconds = 200)
    {
        var start = DateTime.UtcNow;

        var scope = _factory.Services.CreateScope();

        var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        while (DateTime.UtcNow.Subtract(start).TotalSeconds < timeoutInSeconds)
        {
            var messages = await dataContext.Set<OutboxMessage>().AsNoTracking().ToArrayAsync();
            var errors = messages
                .Where(outbox => outbox is { ProcessedDateUtc: not null, Error: not null })
                .Select(outbox => $"DomainEvent '{outbox.Type}': {outbox.Error}").ToArray();
            
            errors.Should().BeEmpty(string.Join("; ", errors));
           
            var consumers = await dataContext.Set<OutboxMessageConsumer>().AsNoTracking().ToArrayAsync();

            if (messages.All(outbox => outbox.ProcessedDateUtc != null && 
                                       consumers.Any(c => c.OutboxMessageId == outbox.Id)))
            {
                return;
            }

            await Task.Delay(delayBetweenAttemptsInMilliseconds);
        }

        throw new InvalidOperationException(
            $"Domain events were not handled within {timeoutInSeconds} second(s). Start: {start}. End: {DateTime.UtcNow}");
    }



    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => _resetDatabase();
}
