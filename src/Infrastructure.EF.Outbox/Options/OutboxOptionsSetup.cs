using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace SolutionTemplate.Infrastructure.EF.Outbox.Options;

public class OutboxOptionsSetup : IConfigureOptions<OutboxOptions>
{
    private readonly IConfiguration _configuration;

    public OutboxOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(OutboxOptions options)
    {
        _configuration.GetSection(nameof(OutboxOptions)).Bind(options);
    }
}
