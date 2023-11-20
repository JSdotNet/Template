using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace SolutionTemplate.Infrastructure.EF.Outbox.Options;

public class OutboxOptionsSetup(IConfiguration configuration) : IConfigureOptions<OutboxOptions>
{
    public void Configure(OutboxOptions options)
    {
        configuration.GetSection(nameof(OutboxOptions)).Bind(options);
    }
}
