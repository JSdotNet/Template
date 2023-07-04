using System.Globalization;
using System.Linq.Expressions;

using SolutionTemplate.Application._;
using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain;
using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Models;

namespace SolutionTemplate.Application.Articles.Queries;

public static class GetArticles
{
    public sealed record Query(string[]? Tags = null, string? SortColumn = null, string? SortOrder = null, int Page = 1, int PageSize = 5) : IQuery<PagedList<Response>>;

    internal sealed class Handler : IQueryHandler<Query, PagedList<Response>>
    {
        private readonly IReadOnlyDataContext _dataContext;

        public Handler(IReadOnlyDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Result<PagedList<Response>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _dataContext.Query<Article>();


            if (request.Tags is { Length: > 0 })
            {
                query = query.Where(article => article.Tags.Any(tag => request.Tags.Any(i => i == tag.Name)));
            }

            Expression<Func<Article, object>> selector = request.SortColumn?.ToLower(CultureInfo.InvariantCulture) switch
            {
                "title" => article => article.Title,
                "createdAt" => article => article.CreatedAt,
                _ => article => article.CreatedAt
            };

            query = request.SortOrder?.ToLower(CultureInfo.InvariantCulture) == "desc" 
                ? query.OrderByDescending(selector) 
                : query.OrderBy(selector);

            var result = await PagedList<Response>.CreateAsync(
                query.Select(article => new Response(article.Title, article.CreatedAt, article.Tags)), 
                request.Page,
                request.PageSize, 
                cancellationToken);
            
            return Result.Success(result);
        }
    }

    public sealed record Response(string Title, DateTime CreatedAt, IReadOnlyList<Tag> Tags);
}