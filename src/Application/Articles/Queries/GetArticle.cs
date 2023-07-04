using Microsoft.EntityFrameworkCore;

using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain;
using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Models;

namespace SolutionTemplate.Application.Articles.Queries;

public static class GetArticle
{
    public sealed record Query(Guid Id) : IQuery<Response>;

    internal sealed class Handler : IQueryHandler<Query, Response>
    {
        private readonly IReadOnlyDataContext _dataContext;

        public Handler(IReadOnlyDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _dataContext.Query<Article>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (result is null)
                return Result.Failure<Response>(ApplicationErrors.NotFound<Article>(request.Id));

            return new Response(result.Id, result.CreatedAt, result.Title, result.Content, result.LastUpdated);
        }
    }

    public sealed record Response(Guid Id, DateTime CreatedAt, string Title, string Content, DateTime LastUpdate);
}
