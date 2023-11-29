using SolutionTemplate.Domain._;

namespace SolutionTemplate.Domain.Tests._;

internal static class AssertionExtensions
{
    internal static ErrorAssertions Should(this Error? instance) => new(instance);
}