
using DemoCID.Application.Abstractions.Message;

namespace DemoCID.Application.UserCases.V1.Commands.Product;

public sealed class CreateProductCommand : ICommand
{
    public string Name { get; set; }
}
