using System;
using Controller.Shared;
using MediatR;
using MediatR.Pipeline;

namespace Controller;

public sealed class GlobalExceptionHandler<TRequest, TResponse, TException>(
    IRequestExceptionNotify notify
) : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : IBaseRequest
    where TResponse : IResult, new()
    where TException : Exception
{
    public Task Handle(
        TRequest request,
        TException exception,
        RequestExceptionHandlerState<TResponse> state,
        CancellationToken cancellationToken
    )
    {
        if (exception is HttpRequestException)
        {
            notify.Notify("Network error, please check your connection and try again");
        }
        else
        {
            notify.Notify(exception.Message);
        }
        state.SetHandled(new() { IsSuccessful = false });
        return Task.CompletedTask;
    }
}
