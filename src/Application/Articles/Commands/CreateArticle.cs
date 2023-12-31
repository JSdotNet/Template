﻿using FluentValidation;

using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Articles;
using SolutionTemplate.Domain.Articles.Repository;
using SolutionTemplate.Domain.Authors.Repository;
using SolutionTemplate.Domain.Services;

namespace SolutionTemplate.Application.Articles.Commands;

public static class CreateArticle
{
    public sealed record Command(string Title, string Content, string Email, string Firstname, string Lastname, params string[] Tags) : ICommand<Response>;

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Title).NotEmpty();
            RuleFor(c => c.Content).NotEmpty();
            RuleFor(c => c.Email).NotEmpty();
            RuleFor(c => c.Firstname).NotEmpty();
            RuleFor(c => c.Lastname).NotEmpty();

            // TODO: Validate tags (can I use domain validation here?)
        }
    }

    internal sealed class Handler(IArticleRepository articleRepository, IAuthorRepository authorRepository) : ICommandHandler<Command, Response>
    {
        public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var author = await authorRepository.FindByEmail(request.Email);
            if (author is null)
            {
                var result = CreateArticleService.CreateArticleForAuthor(request.Title, request.Content, request.Email, request.Firstname, request.Lastname, request.Tags);
                if (result.IsFailure)
                {
                    return Result.Failure<Response>(result.Error!.Value); // TODO I do not like the .Value here...
                }

                articleRepository.Add(result.Value.Article);
                authorRepository.Add(result.Value.Author);

                return new Response(result.Value.Article.Id, result.Value.Article.Title, result.Value.Article.Content);
            }
            else
            {
                var result = Article.Create(request.Title, request.Content, author.Id, request.Tags);
                
                if (result.IsFailure)
                {
                     return Result.Failure<Response>(result.Error!.Value); // TODO I do not like the .Value here...
                }

                articleRepository.Add(result);

                return new Response(result.Value.Id, result.Value.Title, result.Value.Content);
            }
        }
    }

    public sealed record Response(Guid Id, string Title, string Content);
}