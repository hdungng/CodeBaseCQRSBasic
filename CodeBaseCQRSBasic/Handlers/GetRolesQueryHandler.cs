using CodeBaseCQRSBasic.Domain;
using CodeBaseCQRSBasic.Infrastructure;
using CodeBaseCQRSBasic.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeBaseCQRSBasic.Handlers;

public class GetRolesQueryHandler(AppDbContext context) : IRequestHandler<GetRolesQuery, IReadOnlyList<Role>>
{
    public async Task<IReadOnlyList<Role>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await context.Roles
            .Include(x => x.Users)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
