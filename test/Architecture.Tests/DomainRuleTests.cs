using System.Reflection;

using SolutionTemplate.Architecture.Tests.Rules;

namespace SolutionTemplate.Architecture.Tests;

// ReSharper disable once UnusedMember.Global
public sealed class DomainRuleTests : DomainRules
{
    protected override Assembly DomainAssembly => Component.DomainAssembly;

    protected override string[]? DomainAbstractionAssemblies => null;
    protected override string[] Application => Component.ApplicationNamespaces;
    protected override string[] Presentation => Component.PresentationNamespaces;
    protected override string[] Infrastructure => Component.InfrastructureNamespaces;
}