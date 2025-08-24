using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Contract.Services.Product;

public static class Command
{
    public record CreateProduct(string name, decimal price, string description) : ICommand;
    public record UpdateProduct(Guid id, string name, decimal price, string description) : ICommand;
    public record DeleteProduct(Guid id) : ICommand;
}
