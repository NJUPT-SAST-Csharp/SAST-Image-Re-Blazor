using System;
using MediatR;

namespace Controller.Shared;

public interface ICommandSender
{
    public Task<TResponse> CommandAsync<TResponse>(ICommandRequest<TResponse> request)
        where TResponse : IResult;
}

public interface ICommandRequest<out TResponse> : IRequest<TResponse>
    where TResponse : IResult { }

internal interface ICommandRequestHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : ICommandRequest<TResponse>
    where TResponse : IResult { }

internal sealed class InnerCommandSender(IMediator mediator) : ICommandSender
{
    public Task<TResponse> CommandAsync<TResponse>(ICommandRequest<TResponse> request)
        where TResponse : IResult
    {
        return mediator.Send(request);
    }
}
