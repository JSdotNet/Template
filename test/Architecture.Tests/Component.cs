using System.Reflection;

namespace SolutionTemplate.Architecture.Tests;

internal static class Component
{
    private const string Namespace = "Solution_Template";

    internal static string[] DomainNamespaces => new[]
    {
        $"{Namespace}.Domain",
    };

    internal static string[] ApplicationNamespaces => new[]
    {
        $"{Namespace}.Application",
    };

    internal static string[] PresentationNamespaces => new[]
    {
        $"{Namespace}.Presentation",
    };

    internal static string[] InfrastructureNamespaces => new[]
    {
        $"{Namespace}.Infrastructure.EF",
    };


    internal static Assembly DomainAssembly => Domain.AssemblyReference.Assembly;

    //internal static string[]? DomainAbstractionAssemblies => null;
    ////{
    ////    //typeof(Shared.Business.Abstractions.Constants.ClaimConstants).Assembly.FullName!,
    ////    //typeof(Infrastructure.Business.Entity).Assembly.FullName!,
    ////    //typeof(Infrastructure.Utils.ClaimModel).Assembly.FullName!,
    ////};

    internal static Assembly ApplicationAssembly => Application.AssemblyReference.Assembly;
    internal static Assembly[] InfrastructureAssembly => new[] { Infrastructure.EF.AssemblyReference.Assembly };
    internal static Assembly PresentationAssembly => Presentation.Api.AssemblyReference.Assembly;
}
