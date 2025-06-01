using FluentValidation;

namespace NativeChat;

public class NewBotDtoValidation :AbstractValidator<NewBotDto>
{
    public NewBotDtoValidation()
    {
        RuleFor(x => x.Name).NotNull().WithMessage("Name field is required.")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
            .MaximumLength(15).WithMessage("Name cannot exceed 15 characters in length.");

        RuleFor(x => x.LanguageId).NotNull().WithMessage("languageId field is required.")
            .LessThan(6).WithMessage("Maximum languageId is 5")
            .GreaterThanOrEqualTo(0).WithMessage("Minimum languageId is 0");
    }
}
