using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

using SolutionTemplate.Application.Articles.Commands;
using SolutionTemplate.Application.Articles.Queries;

namespace SolutionTemplate.Presentation.Api.Modules;

internal static class ArticleEndpoints
{
    public static void MapArticleEndpoints(this IEndpointRouteBuilder app, IConfiguration _)
    {
        var group = app.MapGroup("article").AllowAnonymous();

        group.MapGet("", GetArticles);
        group.MapGet("{id:guid}", GetArticle);
        group.MapPost("", CreateArticle);
        group.MapPut("", UpdateArticle);
        group.MapDelete("{id:guid}", DeleteArticle);
    }

    private static async Task<IResult> GetArticles(ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetArticles.Query(), cancellationToken);

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> GetArticle(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetArticle.Query(id), cancellationToken);

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> CreateArticle(CreateArticle.Command command, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> UpdateArticle(CreateArticle.Command command, ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);

        return Results.NoContent();
    }

    private static async Task<IResult> DeleteArticle(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteArticle.Command(id), cancellationToken);

        return Results.NoContent();
    }
}
