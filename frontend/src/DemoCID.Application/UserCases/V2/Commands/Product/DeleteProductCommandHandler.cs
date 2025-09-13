

using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V2.Product;
using DemoCICD.Domain.Abstractions.Dappers;
using DemoCICD.Domain.Exceptions;

namespace DemoCICD.Application.UserCases.V2.Commands.Product;

public sealed class DeleteProductCommandHandler : ICommandHandler<Command.DeleteProductCommand>
{
    private readonly IUnitOfWork _unitOfWork; // SQL-SERVER-STRATEGY-2

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id)
            ?? throw new ProductException.ProductNotFoundException(request.Id);

        var result = await _unitOfWork.Products.DeleteAsync(product.Id);

        return Result.Success(result);
    }
}
