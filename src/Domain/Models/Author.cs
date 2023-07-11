using System.Diagnostics;

using SolutionTemplate.Domain._;

namespace SolutionTemplate.Domain.Models;

[DebuggerDisplay("{Id}: {Firstname} {Lastname}")]
public sealed class Author : AggregateRoot
{
    private Author() : base(default!) { }
    private Author(Guid id) : base(id) { }

    public required string Email { get; init; }
    public string Firstname { get; private set; } = default!;
    public string Lastname { get; private set; } = default!;


    public static Result<Author> Create(string email, string firstname, string lastname)
    {
        var author = new Author(Guid.NewGuid())
        {
            Email = email,
            Firstname = firstname,
            Lastname = lastname
        };

        return author;
    }

    public Result Update(string firstName, string lastName)
    {
        Firstname = firstName;
        Lastname = lastName;

        return Result.Success();
    }
}
