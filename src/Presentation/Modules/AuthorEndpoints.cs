using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

using SolutionTemplate.Application.Authors.Commands;
using SolutionTemplate.Application.Authors.Queries;

namespace SolutionTemplate.Presentation.Api.Modules;


public static class AuthorEndpoints 
{
    public static void MapAuthorEndpoints(this IEndpointRouteBuilder app, IConfiguration _)
    {
        var group = app.MapGroup("author").AllowAnonymous();

        group.MapGet("", GetAuthors);
        group.MapGet("{id:guid}", GetAuthor).WithName(nameof(GetAuthor));
        group.MapPost("", CreateAuthor);
        group.MapPut("", UpdateAuthor);
        group.MapDelete("{id:guid}", DeleteAuthor);
    }


    private static async Task<IResult> GetAuthors(ISender sender, CancellationToken cancellationToken)
    {
        var authors = await sender.Send(new GetAuthors.Query(), cancellationToken);

        return TypedResults.Ok(authors.Value);
    }


    private static async Task<Results<Ok<GetAuthor.Response>, NotFound>> GetAuthor(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAuthor.Query(id), cancellationToken);

        return TypedResults.Ok(result.Value);
    }


    private static async Task<IResult> CreateAuthor(CreateAuthor.Command command, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return Results.CreatedAtRoute(nameof(GetAuthor), new { id = result.Value }, result.Value);
    }


    private static async Task<IResult> UpdateAuthor(UpdateAuthor.Command command, ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);

        return Results.NoContent();
    }


    private static async Task<IResult> DeleteAuthor(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteAuthor.Command(id), cancellationToken);

        return Results.NoContent();
    }
}