using FluentValidation;

using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Models;
using SolutionTemplate.Domain.Repository;

namespace SolutionTemplate.Application.Articles.Commands;

public static class DeleteArticle
{
    public sealed record Command(Guid Id) : ICommand;

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Id).NotEmpty(); // TODO Review... Does this check block Guid.Empty?
        }
    }

    internal sealed class Handler : ICommandHandler<Command>
    {
        private readonly IArticleRepository _articleRepository;

        public Handler(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (article is null)
                return Result.Failure(ApplicationErrors.NotFound<Article>(request.Id));

            _articleRepository.Remove(article);

            return Result.Success();
            // Since the repository does not return anything, I create a new Result.
        }
    }
}