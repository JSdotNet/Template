using SolutionTemplate.Domain._;

namespace SolutionTemplate.Application;

public static class ApplicationErrors
{
    internal const string NotFoundCode = "NotFound";
    internal const string AlreadyExistsCode = "AlreadyExists";

    
    public static Error NotFound<T>(Guid id) => new(NotFoundCode, $"{typeof(T).Name} with id {id} not found.");
    public static Error AlreadyExists<T>(string id) => new(AlreadyExistsCode, $"{typeof(T).Name} with id {id} already exists.");
}

