

using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Product;
using DemoCICD.Domain.Abstractions.Dappers;
using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Exceptions;
using DemoCICD.Persistence;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Commands.Product;

public sealed class DeleteProductCommandHandler : ICommandHandler<Command.DeleteProductCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Product, Guid> _productRepository;
    private readonly IUnitOfWork _unitOfWork; // SQL-SERVER-STRATEGY-2
    private readonly ApplicationDbContext _context; // SQL-SERVER-STRATEGY-1
    private readonly IPublisher _publisher;

    public DeleteProductCommandHandler(IRepositoryBase<Domain.Entities.Product, Guid> productRepository,
        IUnitOfWork unitOfWork,
        IPublisher publisher,
        ApplicationDbContext context)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _context = context;
        _publisher = publisher;
    }

    public async Task<Result> Handle(Command.DeleteProductCommand request, CancellationToken cancellationToken)
    {

        var product = await _productRepository.FindByIdAsync(request.Id) ?? throw new ProductException.ProductNotFoundException(request.Id); // Solution 1
        //var product = await _productRepository.FindSingleAsync(x => x.Id.Equals(request.Id)) ?? throw new ProductException.ProductNotFoundException(request.Id); // Solution 2

        _productRepository.Remove(product);

        //await _unitOfWork.SaveChangesAsync(cancellationToken);

        //await _context.SaveChangesAsync(cancellationToken);

        // Send Email
        await _publisher.Publish(new DomainEvent.ProductDeleted(product.Id), cancellationToken);

        return Result.Success();
    }
}
