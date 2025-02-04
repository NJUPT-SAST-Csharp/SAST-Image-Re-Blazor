using System;
using MediatR;
using MediatR.Pipeline;

namespace Controller;

public sealed class GlobalExceptionHandler<TRequest, TResponse>
    : IRequestExceptionHandler<TRequest, TResponse, Exception>
    where TRequest : IBaseRequest
{
    public Task Handle(
        TRequest request,
        Exception exception,
        RequestExceptionHandlerState<TResponse> state,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}
