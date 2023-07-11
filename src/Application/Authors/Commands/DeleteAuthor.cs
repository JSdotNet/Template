using FluentValidation;

using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Models;
using SolutionTemplate.Domain.Repository;

namespace SolutionTemplate.Application.Authors.Commands;

public static class DeleteAuthor
{
    public sealed record Command(Guid Id) : ICommand;

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Id).NotEmpty();
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
            var article = await _authorRepository.GetByIdAsync(request.Id, cancellationToken);
            if (article is null)
                return Result.Failure(ApplicationErrors.NotFound<Author>(request.Id)); // TODO Use AuthorId here

            _authorRepository.Remove(article);

            return Result.Success();
            // Since the repository does not return anything, I create a new Result.
        }
    }
}