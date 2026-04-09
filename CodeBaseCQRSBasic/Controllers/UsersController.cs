using CodeBaseCQRSBasic.Commands;
using CodeBaseCQRSBasic.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeBaseCQRSBasic.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var users = await mediator.Send(new GetUsersQuery(), cancellationToken);
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateUserCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("Route id and payload id must match.");
        }

        var user = await mediator.Send(command, cancellationToken);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await mediator.Send(new DeleteUserCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
