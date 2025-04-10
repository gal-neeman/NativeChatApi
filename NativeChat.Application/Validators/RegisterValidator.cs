using FluentValidation;

namespace NativeChat;

public class RegisterValidator: AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username).NotNull().WithMessage("Name is a required field.")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
            .MaximumLength(20).WithMessage("Name cannot exceed 20 characters in length.");

        RuleFor(x => x.Email).NotNull().WithMessage("Email is a required field.")
            .MinimumLength(10).WithMessage("Email must be at least 10 characters long.")
            .MaximumLength(320).WithMessage("Email cannot exceed 320 characters in length.")
            .Matches("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$").WithMessage("Email must be in proper email format.");

        RuleFor(x => x.FirstName).NotNull().WithMessage("FirstName is a required field.")
            .MinimumLength(3).WithMessage("FirstName must be at least 3 characters long")
            .MaximumLength(20).WithMessage("FirstName cannot exceed 20 characters in length.");

        RuleFor(x => x.LastName).NotNull().WithMessage("LastName is a required field.")
            .MinimumLength(3).WithMessage("LastName must be at least 3 characters long")
            .MaximumLength(40).WithMessage("LastName cannot exceed 40 characters in length.");

        RuleFor(x => x.Password).NotNull().WithMessage("Password is a required field.")
            .MaximumLength(800).WithMessage("Password cannot exceed 800 characters in length.")
            .Must(StrongPassword).WithMessage("Password must have 1 uppercase character, 1 digit and 1 non-alphanumeric character, and be at least 8 characters long.");
    }

    private bool StrongPassword(string? password)
    {
        if (password == null)
            return false;

        // Fail if password is too short
        if (password.Length < 8)
            return false;

        // Fail if: no numeric characters | no characters | non non-alphanumeric characters
        if (
            !password.Any(c => char.IsUpper(c)) ||
            !password.Any(c => char.IsDigit(c)) ||
            !password.Any(c => !char.IsLetterOrDigit(c))
            )
            return false;

        // Passed
        return true;
    }
}
