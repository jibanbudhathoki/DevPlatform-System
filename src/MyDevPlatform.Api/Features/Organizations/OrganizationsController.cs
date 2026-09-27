using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyDevPlatform.Application.Organizations;
using MyDevPlatform.Contracts.Organizations;

namespace MyDevPlatform.Api.Features.Organizations;

[ApiController]
[Authorize(Roles = "Superuser")]
[Route("api/[controller]")]
public class OrganizationsController : ControllerBase
{
    private readonly OrganizationService _organizationService;

    public OrganizationsController(OrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<OrganizationSummaryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var organizations = await _organizationService.GetOrganizationsAsync(cancellationToken);
        return Ok(organizations);
    }

    [HttpPost]
    public async Task<ActionResult<OrganizationDetailsDto>> Create([FromBody] CreateOrganizationRequest request, CancellationToken cancellationToken)
    {
        var organization = await _organizationService.CreateOrganizationAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id = organization.Id }, organization);
    }
}
