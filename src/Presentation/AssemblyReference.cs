using System.Reflection;

namespace SolutionTemplate.Presentation.Api;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}