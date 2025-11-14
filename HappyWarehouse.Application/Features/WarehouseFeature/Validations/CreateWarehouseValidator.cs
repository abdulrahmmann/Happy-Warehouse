using FluentValidation;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Validations;

public class CreateWarehouseValidator: AbstractValidator<CreateWarehouseDto>
{
    public CreateWarehouseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Warehouse name is required.")
            .MaximumLength(60).WithMessage("Warehouse name cannot exceed 60 characters.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Warehouse address is required.")
            .MaximumLength(500).WithMessage("Warehouse address cannot exceed 500 characters.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(60).WithMessage("City cannot exceed 60 characters.");

        RuleFor(x => x.CountryId)
            .GreaterThan(0).WithMessage("Country is required.");

        RuleFor(x => x.CreatedByUserId)
            .GreaterThan(0).WithMessage("CreatedByUserId is required.");
    }
}