using System.Reflection;

using NetArchTest.Rules;

using SolutionTemplate.Architecture.Tests.Extensions;

namespace SolutionTemplate.Architecture.Tests.Rules;

public abstract class ApplicationRules
{
    protected abstract Assembly ApplicationAssembly { get; }

    protected abstract string[] PresentationProjects { get; }
    protected abstract string[] Domain { get; }
    protected abstract string[] Infrastructure { get; }

    [Fact]
    internal void Application_Should_Not_HaveDependencyOnPresentation()
    {
        // Act
        var testResult = Types.InAssembly(ApplicationAssembly).ShouldNot().HaveDependencyOnAny(PresentationProjects).GetResult();


        // Assert
        testResult.AssertSuccessful();
    }


    [Fact(Skip = "How to test if any of the assembly types has the reference")]
    internal void Application_Should_HaveDependencyOnDomain()
    {
        // Act
        var testResult = Types.InAssembly(ApplicationAssembly).Should().HaveDependencyOnAny(Domain).GetResult();


        // Assert
        testResult.AssertSuccessful();
    }

    [Fact]
    internal void Application_Should_Not_HaveDependencyOnInfrastructure()
    {
        // Act
        var testResult = Types.InAssembly(ApplicationAssembly).ShouldNot().HaveDependencyOnAny(Infrastructure).GetResult();


        // Assert
        testResult.AssertSuccessful();
    }
}
