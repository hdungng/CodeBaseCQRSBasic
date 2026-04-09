using CodeBaseCQRSBasic.Domain;
using MediatR;

namespace CodeBaseCQRSBasic.Commands;

public record CreateUserCommand(string UserName, string Email, int RoleId) : IRequest<User>;
