using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace DemoCICD.Contract.Services.Product.Validators;

public class CreateProductValidator : AbstractValidator<Command.CreateProduct>
{
    public CreateProductValidator()
    {
        RuleFor(s => s.name).NotEmpty();
        RuleFor(s => s.price).GreaterThan(0);
        RuleFor(s => s.description).NotEmpty();
    }
}
