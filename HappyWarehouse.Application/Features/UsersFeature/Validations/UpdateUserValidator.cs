using FluentValidation;
using HappyWarehouse.Application.Features.UsersFeature.DTOs;

namespace HappyWarehouse.Application.Features.UsersFeature.Validations;

public class UpdateUserValidator: AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(user => user.UserName)
            .MinimumLength(6).WithMessage("UserName name must be at least 6 characters long.")
            .MaximumLength(60).WithMessage("UserName name cannot exceed 16 characters.");
        
        RuleFor(user => user.FullName)
            .MinimumLength(8).WithMessage("FullName name must be at least 8 characters long.")
            .MaximumLength(160).WithMessage("FullName name cannot exceed 60 characters.");

        RuleFor(user => user.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}