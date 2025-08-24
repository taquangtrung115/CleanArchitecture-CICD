using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace DemoCICD.Contract.Services.Product.Validators;

public class UpdateProductValidator : AbstractValidator<Command.UpdateProduct>
{
    public UpdateProductValidator()
    {
        RuleFor(s => s.name).NotEmpty();
        RuleFor(s => s.price).GreaterThan(0);
        RuleFor(s => s.description).NotEmpty();
    }
}
