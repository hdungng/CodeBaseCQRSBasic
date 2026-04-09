using CodeBaseCQRSBasic.Commands;
using CodeBaseCQRSBasic.Domain;
using CodeBaseCQRSBasic.Infrastructure;
using CodeBaseCQRSBasic.Services;
using MediatR;

namespace CodeBaseCQRSBasic.Handlers;

public class CreateUserCommandHandler(AppDbContext context, IPasswordHasherService passwordHasher)
    : IRequestHandler<CreateUserCommand, User>
{
    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            PasswordHash = passwordHasher.Hash(request.Password),
            RoleId = request.RoleId
        };

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);
        return user;
    }
}
