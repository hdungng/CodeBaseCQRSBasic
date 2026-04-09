using CodeBaseCQRSBasic.Domain;
using MediatR;

namespace CodeBaseCQRSBasic.Commands;

public record UpdateUserCommand(int Id, string UserName, string Email, int RoleId) : IRequest<User?>;
