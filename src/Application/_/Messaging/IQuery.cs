using MediatR;

using SolutionTemplate.Domain._;

namespace SolutionTemplate.Application._.Messaging;

internal interface IQuery<TResponse> : IRequest<Result<TResponse>>;


internal interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;