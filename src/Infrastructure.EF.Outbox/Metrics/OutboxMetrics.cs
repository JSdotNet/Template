using System.Diagnostics.Metrics;


namespace SolutionTemplate.Infrastructure.EF.Outbox.Metrics;

internal class OutboxMetrics
{
    private Counter<int> _unhandledCounter;
    private Counter<int> _totalCounter;


    public OutboxMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create("outbox"); // TODO Review name...

        _unhandledCounter = meter.CreateCounter<int>("outbox.unhandled"); // TODO Maybe use CreateHistogram<int> instead?
        _totalCounter = meter.CreateCounter<int>("outbox.total");
    }

    public void Log(int quantity, int unhandled)
    {
        _totalCounter.Add(quantity);
        _unhandledCounter.Add(unhandled);
    }
}
