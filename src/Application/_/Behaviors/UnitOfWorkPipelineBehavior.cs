﻿
using MediatR;

using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain;
using SolutionTemplate.Domain._;

namespace SolutionTemplate.Application._.Behaviors;

internal sealed class UnitOfWorkPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkPipelineBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // This behavior should only be applied to commands
        var isCommand = typeof(TRequest).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>));
        if (!isCommand)
            return await next();


        //using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var response = await next();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        //transaction.Complete();

        return response;
    }
}