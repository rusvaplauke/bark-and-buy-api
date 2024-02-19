namespace Domain.Exceptions;

public class SellerNotFoundException : Exception
{
    public SellerNotFoundException(int sellerId) : base($"Seller with id {sellerId} not found.") { }
}
