using System;
using MediatR;

namespace Controller.Shared;

public interface ICommandSender
{
    public Task CommandAsync<TCommand>(TCommand request)
        where TCommand : ICommandRequest;
    public Task<TResponse> CommandAsync<TResponse>(ICommandRequest<TResponse> request);
}

public interface ICommandRequest : IRequest { }

public interface ICommandRequest<out TResponse> : IRequest<TResponse> { }

internal interface ICommandRequestHandler<in TRequest> : IRequestHandler<TRequest>
    where TRequest : ICommandRequest { }

internal interface ICommandRequestHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : ICommandRequest<TResponse> { }

internal sealed class InnerCommandSender(IMediator mediator) : ICommandSender
{
    public Task CommandAsync<TCommand>(TCommand request)
        where TCommand : ICommandRequest
    {
        return mediator.Send(request);
    }

    public Task<TResponse> CommandAsync<TResponse>(ICommandRequest<TResponse> request)
    {
        return mediator.Send(request);
    }
}
