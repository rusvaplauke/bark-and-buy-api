namespace Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(int userId, string message) : base($"User with id {userId} not found, {message}.") { }
}
