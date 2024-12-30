using FluentValidation;
using ToDo.Application.DTOs;

namespace ToDo.Application.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().When(x => !x.IsLogin).WithMessage("Name is required.")
                .Length(3, 100).WithMessage("Name must be between 3 and 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(300).WithMessage("Email cannot be longer than 300 characters")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .MaximumLength(50).WithMessage("Password cannot be longer than 50 characters")
                .NotNull().When(x => x.IsEdit).WithMessage("Password is required");
        }
    }
}
