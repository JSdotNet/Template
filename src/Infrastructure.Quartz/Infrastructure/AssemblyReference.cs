﻿using System.Reflection;

namespace SolutionTemplate.Infrastructure.Quartz;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}