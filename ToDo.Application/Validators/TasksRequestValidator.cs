using FluentValidation;
using ToDo.Application.DTOs;

namespace ToDo.Application.Validators
{
    public class TasksRequestValidator : AbstractValidator<TasksRequest>
    {
        public TasksRequestValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(3, 100).WithMessage("Name must be between 3 and 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(4000).WithMessage("Description cannot be longer than 4000 characters");

            RuleFor(x => x.IdUser)
                .NotEmpty().WithMessage("IdUser is required.");

            RuleFor(x => x.Id)
                .NotNull().When(x => x.IsEdit).WithMessage("Id is required");
        }
    }
}
