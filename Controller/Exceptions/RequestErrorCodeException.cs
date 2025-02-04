using System;

namespace Controller.Exceptions;

public sealed class RequestErrorCodeException(string message) : Exception(message) { }
