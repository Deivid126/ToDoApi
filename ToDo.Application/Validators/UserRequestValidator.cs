using FluentValidation;
using ToDo.Application.DTOs;

namespace ToDo.Application.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(3, 100).WithMessage("Name must be between 3 and 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(300).WithMessage("Email cannot be longer than 300 characters")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotNull().When(x => x.IsEdit).WithMessage("Password is required");
        }
    }
}
