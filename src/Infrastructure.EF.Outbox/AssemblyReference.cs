using System.Reflection;

namespace SolutionTemplate.Infrastructure.EF.Outbox;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}