namespace Controller.Exceptions;

public sealed class InvalidRequestException(string message) : Exception(message) { }
