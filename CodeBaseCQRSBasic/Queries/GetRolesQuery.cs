using CodeBaseCQRSBasic.Domain;
using MediatR;

namespace CodeBaseCQRSBasic.Queries;

public record GetRolesQuery() : IRequest<IReadOnlyList<Role>>;
