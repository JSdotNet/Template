using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

using SolutionTemplate.Application._;
using SolutionTemplate.Application.Articles.Commands;
using SolutionTemplate.Application.Articles.Queries;

namespace SolutionTemplate.Presentation.Api.Modules;

internal static class Articles
{
    public static IEndpointRouteBuilder MapArticleEndpoints(this IEndpointRouteBuilder endpoints, IConfiguration _)
    {
        endpoints.MapGet("/article", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetArticles.Query(), cancellationToken);

            return Results.Ok((PagedList<GetArticles.Response>)result);
        }).Produces<PagedList<GetArticles.Response>>().AllowAnonymous();

        //endpoints.MapGet("/articles/{tags:string[]}", async (
        //    string[]? tags,
        //    string? sortColumn,
        //    string? SortOrder,
        //    int page,
        //    int pageSize,
        //    ISender sender, CancellationToken cancellationToken) =>
        //{
        //    var result = await sender.Send(new GetArticles.Query(tags, sortColumn, SortOrder, page, pageSize), cancellationToken);

        //    return Results.Ok(result);
        //});

        endpoints.MapGet("/article/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetArticle.Query(id), cancellationToken);

            return Results.Ok((GetArticle.Response)result);
        }).Produces<GetArticle.Response>().AllowAnonymous();

        endpoints.MapPost("/article", async (CreateArticle.Command command, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);

            return Results.Ok((CreateArticle.Response)result);
        }).Produces<CreateArticle.Response>().AllowAnonymous();

        endpoints.MapPut("/article", async (UpdateArticle.Command command, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(command, cancellationToken);
            
            return Results.NoContent();
        }).AllowAnonymous();

        endpoints.MapDelete("/article/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(new DeleteArticle.Command(id), cancellationToken);

            return Results.NoContent();
        }).AllowAnonymous();


        return endpoints;
    }
}
