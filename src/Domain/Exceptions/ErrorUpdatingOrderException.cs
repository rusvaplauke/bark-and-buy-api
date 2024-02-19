namespace Domain.Exceptions;

public class ErrorUpdatingOrderException : Exception
{
    public ErrorUpdatingOrderException() : base("There was an error updating your order. Please try again, or contact support if the issue persists.") { }
}
