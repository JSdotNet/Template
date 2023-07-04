using FluentValidation;

using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Models;
using SolutionTemplate.Domain.Repository;

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

    internal sealed class Handler : ICommandHandler<Command, Guid>
    {
        private readonly IAuthorRepository _authorRepository;

        public Handler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.FindByEmail(request.Email);
            if (author is not null)
                return Result.Failure<Guid>(ApplicationErrors.AlreadyExists<Author>(request.Email));

            var result = Author.Create(request.Email, request.Firstname, request.Lastname);
            if (result.IsFailure)
            {
                 return Result.Failure<Guid>(result.Error!.Value); // TODO I do not like the .Value here...
            }

            _authorRepository.Add(result);

            return result.Value.Id.Value; // TODO I do not like the .Value.Id.Value here...
        }
    }
}