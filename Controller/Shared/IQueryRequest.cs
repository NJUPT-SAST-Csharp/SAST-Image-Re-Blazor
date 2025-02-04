using System;
using MediatR;

namespace Controller.Shared;

public interface IQuerySender
{
    public Task<TResponse> QueryAsync<TResponse>(IQueryRequest<TResponse> request);
}

public interface IQueryRequest<out TResponse> : IRequest<TResponse> { }

internal interface IQueryRequestHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IQueryRequest<TResponse> { }

internal sealed class InnerQuerySender(IMediator mediator) : IQuerySender
{
    public Task<TResponse> QueryAsync<TResponse>(IQueryRequest<TResponse> request)
    {
        return mediator.Send(request);
    }
}
