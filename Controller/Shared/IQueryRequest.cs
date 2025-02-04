using System;
using MediatR;

namespace Controller.Shared;

public interface IQuerySender
{
    public Task<TResponse> QueryAsync<TResponse>(IQueryRequest<TResponse> request)
        where TResponse : IResult;
}

public interface IQueryRequest<out TResponse> : IRequest<TResponse>
    where TResponse : IResult { }

internal interface IQueryRequestHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IQueryRequest<TResponse>
    where TResponse : IResult { }

internal sealed class InnerQuerySender(IMediator mediator) : IQuerySender
{
    public Task<TResponse> QueryAsync<TResponse>(IQueryRequest<TResponse> request)
        where TResponse : IResult
    {
        return mediator.Send(request);
    }
}
