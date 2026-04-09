using CodeBaseCQRSBasic.Commands;
using CodeBaseCQRSBasic.Domain;
using CodeBaseCQRSBasic.Infrastructure;
using MediatR;

namespace CodeBaseCQRSBasic.Handlers;

public class CreateUserCommandHandler(AppDbContext context) : IRequestHandler<CreateUserCommand, User>
{
    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            RoleId = request.RoleId
        };

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);
        return user;
    }
}
