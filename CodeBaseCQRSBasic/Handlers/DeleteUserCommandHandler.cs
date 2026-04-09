using CodeBaseCQRSBasic.Commands;
using CodeBaseCQRSBasic.Infrastructure;
using MediatR;

namespace CodeBaseCQRSBasic.Handlers;

public class DeleteUserCommandHandler(AppDbContext context) : IRequestHandler<DeleteUserCommand, bool>
{
    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync([request.Id], cancellationToken);
        if (user is null)
        {
            return false;
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
