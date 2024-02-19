namespace Domain.Exceptions;

public class ErrorCreatingOrderException : Exception
{
    public ErrorCreatingOrderException() : base("There was an error creating your order. Please try again, or contact support if the issue persists.") { }
}
