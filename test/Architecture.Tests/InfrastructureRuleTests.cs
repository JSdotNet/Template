using System.Reflection;

using SolutionTemplate.Architecture.Tests.Rules;

namespace SolutionTemplate.Architecture.Tests;

// ReSharper disable once UnusedMember.Global
public sealed class InfrastructureRuleTests : InfrastructureRules
{
    protected override Assembly[] InfrastructureAssemblies => Component.InfrastructureAssembly;

    protected override string[] Application => Component.ApplicationNamespaces;
    protected override string[] Presentation => Component.PresentationNamespaces;
    protected override string[] Domain => Component.DomainNamespaces;
}