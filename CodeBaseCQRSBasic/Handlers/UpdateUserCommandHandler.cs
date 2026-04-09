using CodeBaseCQRSBasic.Commands;
using CodeBaseCQRSBasic.Domain;
using CodeBaseCQRSBasic.Infrastructure;
using MediatR;

namespace CodeBaseCQRSBasic.Handlers;

public class UpdateUserCommandHandler(AppDbContext context) : IRequestHandler<UpdateUserCommand, User?>
{
    public async Task<User?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync([request.Id], cancellationToken);
        if (user is null)
        {
            return null;
        }

        user.UserName = request.UserName;
        user.Email = request.Email;
        user.RoleId = request.RoleId;

        await context.SaveChangesAsync(cancellationToken);
        return user;
    }
}
