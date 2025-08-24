using DemoCICD.Domain.Abstractions.Dappers.Repositories.Product;


namespace DemoCICD.Domain.Abstractions.Dappers;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
}
