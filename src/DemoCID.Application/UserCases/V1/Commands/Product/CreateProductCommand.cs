
using DemoCICD.Application.Abstractions.Message;

namespace DemoCICD.Application.UserCases.V1.Commands.Product;

public sealed class CreateProductCommand : ICommand
{
    public string Name { get; set; }
}
