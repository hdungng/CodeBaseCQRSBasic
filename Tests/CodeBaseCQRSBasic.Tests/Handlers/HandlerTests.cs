using CodeBaseCQRSBasic.Commands;
using CodeBaseCQRSBasic.Domain;
using CodeBaseCQRSBasic.Handlers;
using CodeBaseCQRSBasic.Queries;
using CodeBaseCQRSBasic.Tests.Common;
using FluentAssertions;

namespace CodeBaseCQRSBasic.Tests.Handlers;

public class HandlerTests
{
    [Fact]
    public async Task CreateRoleCommandHandler_Should_Create_Role()
    {
        await using var context = TestDbContextFactory.Create();
        var handler = new CreateRoleCommandHandler(context);

        var result = await handler.Handle(new CreateRoleCommand("Admin"), CancellationToken.None);

        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("Admin");
    }

    [Fact]
    public async Task CreateUserCommandHandler_Should_Create_User()
    {
        await using var context = TestDbContextFactory.Create();
        context.Roles.Add(new Role { Id = 1, Name = "User" });
        await context.SaveChangesAsync();

        var handler = new CreateUserCommandHandler(context);
        var result = await handler.Handle(new CreateUserCommand("john", "john@mail.com", 1), CancellationToken.None);

        result.Id.Should().BeGreaterThan(0);
        context.Users.Count().Should().Be(1);
    }

    [Fact]
    public async Task UpdateUserCommandHandler_Should_Update_Existing_User()
    {
        await using var context = TestDbContextFactory.Create();
        context.Roles.AddRange(new Role { Id = 1, Name = "User" }, new Role { Id = 2, Name = "Admin" });
        context.Users.Add(new User { Id = 10, UserName = "u1", Email = "u1@mail.com", RoleId = 1 });
        await context.SaveChangesAsync();

        var handler = new UpdateUserCommandHandler(context);
        var result = await handler.Handle(new UpdateUserCommand(10, "updated", "updated@mail.com", 2), CancellationToken.None);

        result.Should().NotBeNull();
        result!.UserName.Should().Be("updated");
        result.RoleId.Should().Be(2);
    }

    [Fact]
    public async Task DeleteUserCommandHandler_Should_Delete_Existing_User()
    {
        await using var context = TestDbContextFactory.Create();
        context.Roles.Add(new Role { Id = 1, Name = "User" });
        context.Users.Add(new User { Id = 1, UserName = "u1", Email = "u1@mail.com", RoleId = 1 });
        await context.SaveChangesAsync();

        var handler = new DeleteUserCommandHandler(context);
        var result = await handler.Handle(new DeleteUserCommand(1), CancellationToken.None);

        result.Should().BeTrue();
        context.Users.Should().BeEmpty();
    }

    [Fact]
    public async Task GetUsersQueryHandler_Should_Return_Users()
    {
        await using var context = TestDbContextFactory.Create();
        context.Roles.Add(new Role { Id = 1, Name = "User" });
        context.Users.AddRange(
            new User { Id = 1, UserName = "u1", Email = "u1@mail.com", RoleId = 1 },
            new User { Id = 2, UserName = "u2", Email = "u2@mail.com", RoleId = 1 });
        await context.SaveChangesAsync();

        var handler = new GetUsersQueryHandler(context);
        var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetRolesQueryHandler_Should_Return_Roles()
    {
        await using var context = TestDbContextFactory.Create();
        context.Roles.AddRange(new Role { Id = 1, Name = "User" }, new Role { Id = 2, Name = "Admin" });
        await context.SaveChangesAsync();

        var handler = new GetRolesQueryHandler(context);
        var result = await handler.Handle(new GetRolesQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
    }
}
