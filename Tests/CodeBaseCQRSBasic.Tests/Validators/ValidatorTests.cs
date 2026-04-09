using CodeBaseCQRSBasic.Commands;
using CodeBaseCQRSBasic.Validators;
using FluentValidation.TestHelper;

namespace CodeBaseCQRSBasic.Tests.Validators;

public class ValidatorTests
{
    [Fact]
    public void UserValidator_Should_Have_Error_For_Invalid_Data()
    {
        var validator = new UserValidator();
        var result = validator.TestValidate(new CreateUserCommand("", "bad-email", "short", 0));

        result.ShouldHaveValidationErrorFor(x => x.UserName);
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Password);
        result.ShouldHaveValidationErrorFor(x => x.RoleId);
    }

    [Fact]
    public void UserValidator_Should_Not_Have_Error_For_Valid_Data()
    {
        var validator = new UserValidator();
        var result = validator.TestValidate(new CreateUserCommand("john", "john@mail.com", "Password123!", 1));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void UpdateUserValidator_Should_Have_Error_For_Invalid_Data()
    {
        var validator = new UpdateUserValidator();
        var result = validator.TestValidate(new UpdateUserCommand(0, "", "bad-email", 0));

        result.ShouldHaveValidationErrorFor(x => x.Id);
        result.ShouldHaveValidationErrorFor(x => x.UserName);
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.RoleId);
    }

    [Fact]
    public void RoleValidator_Should_Have_Error_For_Empty_Name()
    {
        var validator = new RoleValidator();
        var result = validator.TestValidate(new CreateRoleCommand(""));

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void RoleValidator_Should_Not_Have_Error_For_Valid_Name()
    {
        var validator = new RoleValidator();
        var result = validator.TestValidate(new CreateRoleCommand("Admin"));

        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}
