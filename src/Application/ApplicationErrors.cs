using SolutionTemplate.Domain._;

namespace SolutionTemplate.Application;

public static class ApplicationErrors
{
    public enum Code
    {
        NotFound,
        AlreadyExists
    }

    public static Error NotFound<T>(Guid id) => Create(Code.NotFound, $"{typeof(T).Name} with id {id} not found.");
    public static Error AlreadyExists<T>(string id) => Create(Code.AlreadyExists, $"{typeof(T).Name} with id {id} already exists.");


    private static Error Create(Code code, string message) => new(code.ToString(), message);
}

