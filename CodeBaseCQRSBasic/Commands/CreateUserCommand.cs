using CodeBaseCQRSBasic.Domain;
using MediatR;

namespace CodeBaseCQRSBasic.Commands;

public record CreateUserCommand(string UserName, string Email, string Password, int RoleId) : IRequest<User>;
