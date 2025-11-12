using FluentValidation;
using HappyWarehouse.Application.Features.WarehouseItemFeature.DTOs;

namespace HappyWarehouse.Application.Features.WarehouseItemFeature.Validators;

public class CreateWarehouseItemValidator: AbstractValidator<CreateWarehouseItemDto>
{
    public CreateWarehouseItemValidator()
    {
        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("Item name is required.")
            .MaximumLength(150).WithMessage("Item name must not exceed 150 characters.");

        RuleFor(x => x.SkuCode)
            .MaximumLength(100).WithMessage("SKU code must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.SkuCode));

        RuleFor(x => x.Qty)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.CostPrice)
            .GreaterThan(0).WithMessage("Cost price must be greater than zero.")
            .PrecisionScale(18, 2, false).WithMessage("Cost price must be a decimal with up to 2 decimal places.");

        RuleFor(x => x.MsrpPrice)
            .GreaterThanOrEqualTo(0).When(x => x.MsrpPrice.HasValue)
            .WithMessage("MSRP price cannot be negative.")
            .PrecisionScale(18, 2, false).WithMessage("MSRP price must be a decimal with up to 2 decimal places.");

        RuleFor(x => x.WarehouseId)
            .GreaterThan(0).WithMessage("Warehouse ID must be greater than zero.");

        RuleFor(x => x.CreatedByUserId)
            .GreaterThan(0).WithMessage("CreatedByUserId must be greater than zero.");

        RuleFor(x => x.CreatedBy)
            .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.CreatedBy))
            .WithMessage("CreatedBy name must not exceed 100 characters.");
    }
}