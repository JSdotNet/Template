using FluentValidation;

using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Models;
using SolutionTemplate.Domain.Repository;

namespace SolutionTemplate.Application.Articles.Commands;

public static class UpdateArticle
{
    public sealed record Command(Guid Id, string Title, string Content) : ICommand;

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.Title).NotEmpty();
            RuleFor(c => c.Content).NotEmpty();
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


            return article.Update(request.Title, request.Content);
            // Here I chose not to return the article or Id, that means I can just forward the result from the domain model.
        }
    }
}