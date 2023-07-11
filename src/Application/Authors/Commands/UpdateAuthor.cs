using FluentValidation;

using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Models;
using SolutionTemplate.Domain.Repository;

namespace SolutionTemplate.Application.Authors.Commands;

public static class UpdateAuthor
{
    public sealed record Command(Guid Id, string Firstname, string Lastname) : ICommand;

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.Firstname).NotEmpty();
            RuleFor(c => c.Lastname).NotEmpty();
        }
    }

    internal sealed class Handler : ICommandHandler<Command>
    {
        private readonly IAuthorRepository _authorRepository;

        public Handler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(request.Id, cancellationToken);
            if (author is null)
                return Result.Failure(ApplicationErrors.NotFound<Author>(request.Id));


            return author.Update(request.Firstname, request.Lastname);
            // Here I chose not to return the article or Id, that means I can just forward the result from the domain model.
        }
    }
}