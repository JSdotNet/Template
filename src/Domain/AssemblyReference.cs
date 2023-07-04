using System.Reflection;

namespace SolutionTemplate.Domain;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}