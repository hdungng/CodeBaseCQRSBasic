using CodeBaseCQRSBasic.Commands;
using CodeBaseCQRSBasic.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeBaseCQRSBasic.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var roles = await mediator.Send(new GetRolesQuery(), cancellationToken);
        return Ok(roles);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = role.Id }, role);
    }
}
