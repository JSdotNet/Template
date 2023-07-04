using Microsoft.EntityFrameworkCore;

using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain;
using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Models;

namespace SolutionTemplate.Application.Authors.Queries;

public static class GetAuthors
{
    public sealed record Query : IQuery<IList<Response>>;

    internal sealed class Handler : IQueryHandler<Query, IList<Response>>
    {
        private readonly IReadOnlyDataContext _dataContext;

        public Handler(IReadOnlyDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Result<IList<Response>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _dataContext.Query<Author>();

            var result = await query.ToListAsync(cancellationToken);
            
            return result.Select(r => new Response(r.Id, r.Firstname, r.Lastname)).ToList();
        }
    }

    public sealed record Response(Guid Id, string Firstname, string Lastname);
}