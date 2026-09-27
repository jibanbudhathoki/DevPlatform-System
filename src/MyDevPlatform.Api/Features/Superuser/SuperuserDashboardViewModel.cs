using MyDevPlatform.Contracts.Organizations;

namespace MyDevPlatform.Api.Features.Superuser;

public record SuperuserDashboardViewModel(
    IReadOnlyCollection<OrganizationSummaryDto> Organizations,
    bool IsApiAvailable,
    string? ErrorMessage,
    string? SearchTerm = null);