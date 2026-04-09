using CodeBaseCQRSBasic.Commands;
using FluentValidation;

namespace CodeBaseCQRSBasic.Validators;

public class RoleValidator : AbstractValidator<CreateRoleCommand>
{
    public RoleValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
