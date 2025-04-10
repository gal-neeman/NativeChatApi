using FluentValidation;

namespace NativeChat;

public class CredentialsValidator : AbstractValidator<CredentialsDto>
{
    public CredentialsValidator()
    {
        RuleFor(x => x.Email).NotNull().WithMessage("Email field is required.")
            .MinimumLength(10).WithMessage("Email must be at least 10 characters long.")
            .MaximumLength(320).WithMessage("Email cannot exceed 320 characters in length.")
            .Matches("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$").WithMessage("Email must be in proper email format."); // Email regex: ^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$

        RuleFor(x => x.Password).NotNull().WithMessage("Password field is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(800).WithMessage("Password cannot exceed 800 characters in length.");
    }
}
