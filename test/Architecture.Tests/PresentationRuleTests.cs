using System.Reflection;

using SolutionTemplate.Architecture.Tests.Rules;

namespace SolutionTemplate.Architecture.Tests;

// ReSharper disable once UnusedMember.Global
public sealed class PresentationRuleTests : PresentationRules
{
    protected override Assembly PresentationAssembly => Component.PresentationAssembly;

    protected override string[] Infrastructure => Component.InfrastructureNamespaces;
    protected override string[] Application => Component.ApplicationNamespaces;
}