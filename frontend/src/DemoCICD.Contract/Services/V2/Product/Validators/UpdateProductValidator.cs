using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Contract.Services.V2.Product;
using FluentValidation;

namespace DemoCICD.Contract.Services.V2.Product.Validators;

public class UpdateProductValidator : AbstractValidator<Command.UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(s => s.Name).NotEmpty();
        RuleFor(s => s.Price).GreaterThan(0);
        RuleFor(s => s.Description).NotEmpty();
    }
}
