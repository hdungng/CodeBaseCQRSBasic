using CodeBaseCQRSBasic.Domain;
using MediatR;

namespace CodeBaseCQRSBasic.Commands;

public record CreateRoleCommand(string Name) : IRequest<Role>;
