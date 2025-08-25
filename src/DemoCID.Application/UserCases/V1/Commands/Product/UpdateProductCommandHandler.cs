

using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Product;
using DemoCICD.Domain.Abstractions.Dappers;
using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Exceptions;
using DemoCICD.Persistence;

namespace DemoCICD.Application.UserCases.V1.Commands.Product;

public sealed class UpdateProductCommandHandler : ICommandHandler<Command.UpdateProductCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Product, Guid> _productRepository;
    private readonly IUnitOfWork _unitOfWork; // SQL-SERVER-STRATEGY-2
    private readonly ApplicationDbContext _context; // SQL-SERVER-STRATEGY-1

    public UpdateProductCommandHandler(IRepositoryBase<Domain.Entities.Product, Guid> productRepository,
        IUnitOfWork unitOfWork,
        ApplicationDbContext context)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }
    public async Task<Result> Handle(Command.UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByIdAsync(request.Id) ?? throw new ProductException.ProductNotFoundException(request.Id); // Solution 1
        //var product = await _productRepository.FindSingleAsync(x => x.Id.Equals(request.Id)) ?? throw new ProductException.ProductNotFoundException(request.Id); // Solution 2

        //product.Name = request.Name;
        //product.Description = request.Description;
        //product.Price = request.Price;

        product.Update(request.Name, request.Price, request.Description);

        //await _unitOfWork.SaveChangesAsync(cancellationToken);
        //await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
