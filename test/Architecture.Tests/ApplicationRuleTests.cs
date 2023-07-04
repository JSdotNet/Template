using System.Reflection;

using SolutionTemplate.Architecture.Tests.Rules;

namespace SolutionTemplate.Architecture.Tests;

public sealed class ApplicationRuleTests : ApplicationRules
{
    protected override Assembly ApplicationAssembly => Component.ApplicationAssembly;

    protected override string[] PresentationProjects => Component.PresentationNamespaces;
    protected override string[] Domain => Component.DomainNamespaces;
    protected override string[] Infrastructure => Component.InfrastructureNamespaces;
}
