namespace ExceptionFlow;

public sealed class ValidationException(string message) : Exception(message);