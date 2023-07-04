using System.Collections;
using System.Reflection;

using NetArchTest.Rules;

using SolutionTemplate.Architecture.Tests.Extensions;

using Xunit;

namespace SolutionTemplate.Architecture.Tests.Rules;


public class InfrastructureAssemblyListTestDataGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = Component.InfrastructureAssembly.Select(assembly => new object[] { assembly }).ToList();

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public abstract class InfrastructureRules
{
    protected abstract Assembly[] InfrastructureAssemblies { get; }

    protected abstract string[] Application { get; }
    protected abstract string[] Presentation { get; }
    protected abstract string[] Domain { get; }

    [Theory]
    [ClassData(typeof(InfrastructureAssemblyListTestDataGenerator))]
    internal void Infrastructure_Should_Not_HaveDependencyOnApplication(Assembly assembly)
    {
        // Act
        var testResult = Types.InAssembly(assembly).ShouldNot().HaveDependencyOnAny(Application).GetResult();


        // Assert
        testResult.AssertSuccessful();
    }

    [Theory]
    [ClassData(typeof(InfrastructureAssemblyListTestDataGenerator))]
    internal void Infrastructure_Should_Not_HaveDependencyOnPresentation(Assembly assembly)
    {
        // Act
        var testResult = Types.InAssembly(assembly).ShouldNot().HaveDependencyOnAny(Presentation).GetResult();


        // Assert
        testResult.AssertSuccessful();
    }


    [Theory(Skip = "How can we check if any type any of the assemblies has a reference")]
    [ClassData(typeof(InfrastructureAssemblyListTestDataGenerator))]
    internal void Infrastructure_Should_HaveDependencyOnDomain(Assembly assembly)
    {
        // Act
        var testResult = Types.InAssembly(assembly).Should().HaveDependencyOnAny(Domain).GetResult();


        // Assert
        testResult.AssertSuccessful();
    }
}
