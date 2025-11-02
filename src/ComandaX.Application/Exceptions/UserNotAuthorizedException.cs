namespace ComandaX.Application.Exceptions;

public sealed class UserNotAuthorizedException(string email) : Exception($"User with email {email} is not authorized.");
