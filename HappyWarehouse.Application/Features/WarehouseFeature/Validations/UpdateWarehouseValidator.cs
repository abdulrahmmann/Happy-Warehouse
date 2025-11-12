using FluentValidation;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Validations;

public class UpdateWarehouseValidator: AbstractValidator<UpdateWarehouseDto>
{
    public UpdateWarehouseValidator()
    {
        RuleFor(x => x.Name).MaximumLength(60).WithMessage("Warehouse name cannot exceed 100 characters.");

        RuleFor(x => x.Address).MaximumLength(500).WithMessage("Warehouse address cannot exceed 250 characters.");

        RuleFor(x => x.City).MaximumLength(60).WithMessage("City cannot exceed 100 characters.");

        RuleFor(x => x.CountryId).GreaterThan(0);
    }
}