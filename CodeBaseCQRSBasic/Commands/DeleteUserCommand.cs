using MediatR;

namespace CodeBaseCQRSBasic.Commands;

public record DeleteUserCommand(int Id) : IRequest<bool>;
