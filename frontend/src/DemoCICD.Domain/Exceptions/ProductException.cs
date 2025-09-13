
namespace DemoCICD.Domain.Exceptions;

public static class ProductException
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid productId)
            : base($"The product with the id {productId} was not found.") { }
    }
}
