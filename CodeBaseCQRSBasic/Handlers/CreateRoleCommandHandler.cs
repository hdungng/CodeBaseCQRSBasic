using CodeBaseCQRSBasic.Commands;
using CodeBaseCQRSBasic.Domain;
using CodeBaseCQRSBasic.Infrastructure;
using MediatR;

namespace CodeBaseCQRSBasic.Handlers;

public class CreateRoleCommandHandler(AppDbContext context) : IRequestHandler<CreateRoleCommand, Role>
{
    public async Task<Role> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role { Name = request.Name };
        context.Roles.Add(role);
        await context.SaveChangesAsync(cancellationToken);
        return role;
    }
}
