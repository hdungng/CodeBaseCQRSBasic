using CodeBaseCQRSBasic.Domain;
using MediatR;

namespace CodeBaseCQRSBasic.Queries;

public record GetUsersQuery() : IRequest<IReadOnlyList<User>>;
