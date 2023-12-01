using FluentValidation;

using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Authors;
using SolutionTemplate.Domain.Authors.Repository;

namespace SolutionTemplate.Application.Authors.Commands;

public static class CreateAuthor
{
    public sealed record Command(string Email, string Firstname, string Lastname) : ICommand<Guid>;

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Email).NotEmpty();
            RuleFor(c => c.Firstname).NotEmpty();
            RuleFor(c => c.Lastname).NotEmpty();
        }
    }

    internal sealed class Handler(IAuthorRepository authorRepository) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var author = await authorRepository.FindByEmail(request.Email);
            if (author is not null)
                return Result.Failure<Guid>(ApplicationErrors.AlreadyExists<Author>(request.Email));

            var result = Author.Create(request.Email, request.Firstname, request.Lastname);
            if (result.IsFailure)
                return Result.Failure<Guid>(result.Error!.Value); // TODO I do not like the .Value here...

            authorRepository.Add(result);

            return result.Value.Id; // TODO I do not like the .Value.Id here...
        }
    }
}