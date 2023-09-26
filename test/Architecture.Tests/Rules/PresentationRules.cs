using System.Reflection;

using NetArchTest.Rules;

using SolutionTemplate.Architecture.Tests.Extensions;

namespace SolutionTemplate.Architecture.Tests.Rules;

public abstract class PresentationRules
{
    protected abstract Assembly PresentationAssembly { get; }

    protected abstract string[] Infrastructure { get; }
    protected abstract string[] Application { get; }

    [Fact]
    internal void Presentation_Should_Not_HaveDependencyOnInfrastructure()
    {
        // Act
        var testResult = Types.InAssembly(PresentationAssembly).ShouldNot().HaveDependencyOnAny(Infrastructure).GetResult();


        // Assert
        testResult.AssertSuccessful();
    }


    [Fact(Skip = "Can we test this?")]
    internal void Presentation_Should_HaveDependencyOnApplication()
    {
        // Act
        var testResult = Types.InAssembly(PresentationAssembly).Should().HaveDependencyOnAny(Application).GetResult();


        // Assert
        testResult.AssertSuccessful();
    }
}
