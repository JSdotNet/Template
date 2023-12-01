using Microsoft.EntityFrameworkCore;

using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain;
using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Authors;

namespace SolutionTemplate.Application.Authors.Queries;

public static class GetAuthor
{
    public sealed record Query(Guid Id) : IQuery<Response>;

    internal sealed class Handler(IReadOnlyDataContext dataContext) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dataContext.Query<Author>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (result is null)
                return Result.Failure<Response>(ApplicationErrors.NotFound<Author>(request.Id)); // TODO Review use of Error

            return new Response(result.Id, result.Email, result.Firstname, result.Lastname);
        }
    }

    public sealed record Response(Guid Id, string Email, string Firstname, string Lastname);
}
