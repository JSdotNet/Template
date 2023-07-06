using System.Diagnostics;

using SolutionTemplate.Domain._;

namespace SolutionTemplate.Domain.Models;

public sealed record AuthorId(Guid Value) : AggregateRootId(Value);

[DebuggerDisplay("{Id}: {Firstname} {Lastname}")]
public sealed class Author : AggregateRoot<AuthorId>
{
    private Author() : base(default!) { }
    private Author(AuthorId id) : base(id) { }

    public required string Email { get; init; }
    public string Firstname { get; private set; } = default!;
    public string Lastname { get; private set; } = default!;


    public static Result<Author> Create(string email, string firstname, string lastname)
    {
        var author = new Author(new AuthorId(Guid.NewGuid()))
        {
            Email = email,
            Firstname = firstname,
            Lastname = lastname
        };

        author.RaiseDomainEvent(new DomainEvents.AuthorCreated(author.Id));

        return author;
    }

    public Result Update(string firstName, string lastName)
    {
        Firstname = firstName;
        Lastname = lastName;

        return Result.Success();
    }
}
