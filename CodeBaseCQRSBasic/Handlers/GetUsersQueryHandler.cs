using CodeBaseCQRSBasic.Domain;
using CodeBaseCQRSBasic.Infrastructure;
using CodeBaseCQRSBasic.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeBaseCQRSBasic.Handlers;

public class GetUsersQueryHandler(AppDbContext context) : IRequestHandler<GetUsersQuery, IReadOnlyList<User>>
{
    public async Task<IReadOnlyList<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await context.Users
            .Include(x => x.Role)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
