using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Entities.Constants;
using Kuva.Auth.Entities.Dtos.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kuva.Auth.Service.Controllers;

[ApiController]
[Authorize(Roles = RoleNames.KuvaAdmin)]
[Route("api/v1/auth/internal")]
public sealed class InternalStoreOperatorsController(IStoreOperatorService storeOperatorService) : ControllerBase
{
    [HttpPost("stores/{storeId:guid}/operators")]
    public async Task<IActionResult> Create(Guid storeId, CreateStoreOperatorRequest request, CancellationToken cancellationToken)
    {
        var response = await storeOperatorService.CreateAsync(storeId, request, HttpContext.ToRequestContext(), cancellationToken);
        return CreatedAtAction(nameof(List), new { storeId }, response);
    }

    [HttpGet("stores/{storeId:guid}/operators")]
    public async Task<IActionResult> List(Guid storeId, CancellationToken cancellationToken)
    {
        var response = await storeOperatorService.ListByStoreAsync(storeId, cancellationToken);
        return Ok(response);
    }

    [HttpPatch("operators/{operatorId:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid operatorId, UpdateStoreOperatorStatusRequest request, CancellationToken cancellationToken)
    {
        await storeOperatorService.UpdateStatusAsync(operatorId, request.Status, HttpContext.ToRequestContext(), cancellationToken);
        return NoContent();
    }
}
