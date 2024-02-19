namespace Domain.Exceptions;

public class CleanupServiceException : Exception
{
    public CleanupServiceException(string message) : base($"There were some issues with periodic cleanup: {message}. " +
                                                        $"Please try again later or contact support if the problem persists.")
    { }
}
