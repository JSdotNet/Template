using NetArchTest.Rules;

using Xunit;

namespace SolutionTemplate.Architecture.Tests.Extensions;

internal static class TestResultExtensions
{
    internal static void AssertSuccessful(this TestResult testResult)
    {
        var print = testResult.FailingTypeNames != null
            ? string.Join(Environment.NewLine, testResult.FailingTypeNames)
            : null;

        Assert.True(testResult.IsSuccessful, print);
    }
}
