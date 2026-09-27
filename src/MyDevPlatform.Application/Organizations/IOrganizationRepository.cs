using MyDevPlatform.Contracts.Organizations;
using MyDevPlatform.Domain.Organizations;

namespace MyDevPlatform.Application.Organizations;

public interface IOrganizationRepository
{
    Task<IReadOnlyCollection<OrganizationSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Organization?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Organization> AddAsync(Organization organization, CancellationToken cancellationToken = default);
}
