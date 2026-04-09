using CodeBaseCQRSBasic.Commands;
using FluentValidation;

namespace CodeBaseCQRSBasic.Validators;

public class UserValidator : AbstractValidator<CreateUserCommand>
{
    public UserValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.RoleId).GreaterThan(0);
    }
}
