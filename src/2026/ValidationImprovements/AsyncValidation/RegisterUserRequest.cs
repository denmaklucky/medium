namespace AsyncValidation;

public sealed record RegisterUserRequest([UniqueUsername] string Username);
