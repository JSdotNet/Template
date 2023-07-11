using System.Globalization;
using System.Reflection;

using NetArchTest.Rules;

using SolutionTemplate.Architecture.Tests.Extensions;
using SolutionTemplate.Domain._;

using Xunit;

namespace SolutionTemplate.Architecture.Tests.Rules;

public abstract class DomainRules
{
    protected abstract Assembly DomainAssembly { get; }
    protected abstract string[]? DomainAbstractionAssemblies { get; }

    protected abstract string[] Application { get; }
    protected abstract string[] Presentation { get; }
    protected abstract string[] Infrastructure { get; }


    [Fact]
    internal void Domain_Should_Not_HaveDependencyOnApplication()
    {
        // Act
        var testResult = Types.InAssembly(DomainAssembly).ShouldNot().HaveDependencyOnAny(Application).GetResult();

        // Assert
        testResult.AssertSuccessful();
    }

    [Fact]
    internal void Domain_Should_Not_HaveDependencyOnPresentation()
    {
        // Act
        var testResult = Types.InAssembly(DomainAssembly).ShouldNot().HaveDependencyOnAny(Presentation).GetResult();

        // Assert
        testResult.AssertSuccessful();
    }

    [Fact]
    internal void Domain_Should_Not_HaveDependencyOnInfrastructure()
    {
        // Act
        var testResult = Types.InAssembly(DomainAssembly).ShouldNot().HaveDependencyOnAny(Infrastructure).GetResult();

        // Assert
        testResult.AssertSuccessful();
    }


    [Fact]
    internal void Domain_Should_Not_Reference_PackagesOrProjects()
    {
        // Act
        var referencedAssemblies = DomainAssembly.GetReferencedAssemblies()
            .Where(x => 
                !x.Name!.StartsWith("System", true, CultureInfo.InvariantCulture) && 
                !x.Name.StartsWith("Microsoft", true, CultureInfo.InvariantCulture)).ToArray();

        // Assert
        Assert.Empty(referencedAssemblies);
    }




    [Fact]
    internal void Domain_Should_OnlyReferenceDomainAbstractions()
    {
        // Arrange
        if (DomainAbstractionAssemblies is null)
            return;

        // Act
        var testResult = Types.InAssembly(DomainAssembly).ShouldNot().HaveDependenciesOtherThan(DomainAbstractionAssemblies).GetResult();

        // Assert
        testResult.AssertSuccessful();
    }



    // MODELING RULES

    [Fact] // TODO (Skip = "Can we test this?")]
    internal void DomainEntities_Should_Not_BeMutable()
    {
        // Act
        var testResult = Types.InAssembly(DomainAssembly).That().ResideInNamespaceContaining("Entities").Should().BeImmutable().GetResult();


        // Assert
        testResult.AssertSuccessful();
    }

    [Fact] // TODO (Skip = "Can we test this?")]
    internal void DomainEntities_Should_InheritFromEntity()
    {
        // Act
        var testResult = Types.InAssembly(DomainAssembly).That().ResideInNamespaceContaining("Entities").Should().Inherit(typeof(Entity))
            .Or().NotBeClasses() // TODO Check if it is a record?
            .GetResult();


        // Assert
        testResult.AssertSuccessful();
    }

    [Fact]
    internal void DomainEntities_Should_Not_BeInterfaces()
    {
        // Act
        var testResult = Types.InAssembly(DomainAssembly).That().ResideInNamespaceContaining("Entities").ShouldNot().BeInterfaces().GetResult();


        // Assert
        testResult.AssertSuccessful();
    }

    [Fact]
    internal void DomainEntities_Should_BeAbstractOrSealed()
    {
        // Act
        var testResult = Types.InAssembly(DomainAssembly).That().ResideInNamespaceContaining("Entities").Should().BeAbstract().Or().BeSealed().GetResult();

        // Assert
        testResult.AssertSuccessful();
    }

    [Fact(Skip = "Can we test this?")] // TODO ...
    internal void DomainEntities_Should_Not_HavePublicConstructor()
    {
        // Act
        var testResult = Types.InAssembly(DomainAssembly).That().ResideInNamespaceContaining("Entities").ShouldNot().BeInterfaces().GetResult();


        //// Assert
        //testResult.AssertSuccessful();
    }
}
