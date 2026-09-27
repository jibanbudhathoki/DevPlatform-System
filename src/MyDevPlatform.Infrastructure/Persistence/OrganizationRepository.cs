using Microsoft.EntityFrameworkCore;
using MyDevPlatform.Application.Organizations;
using MyDevPlatform.Contracts.Organizations;
using MyDevPlatform.Domain.Organizations;

namespace MyDevPlatform.Infrastructure.Persistence;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly MyDevPlatformDbContext _dbContext;

    public OrganizationRepository(MyDevPlatformDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<OrganizationSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Organizations
            .AsNoTracking()
            .Include(x => x.Subscription)
            .Select(x => new OrganizationSummaryDto(
                x.Id,
                x.Name,
                x.Slug,
                x.Email,
                x.Status.ToString(),
                x.Subscription != null ? x.Subscription.Status.ToString() : null,
                x.CreatedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<Organization?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Organizations
            .Include(x => x.Subscription)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<bool> SlugExistsAsync(string slug, Guid? exceptId = null, CancellationToken cancellationToken = default)
    {
        return _dbContext.Organizations.AnyAsync(
            organization => organization.Slug == slug && (!exceptId.HasValue || organization.Id != exceptId.Value),
            cancellationToken);
    }

    public Task<bool> HasProjectsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Projects.AnyAsync(project => project.OrganizationId == id, cancellationToken);
    }

    public async Task<Organization> AddAsync(Organization organization, CancellationToken cancellationToken = default)
    {
        _dbContext.Organizations.Add(organization);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return organization;
    }

    public async Task<Organization> UpdateAsync(Organization organization, CancellationToken cancellationToken = default)
    {
        organization.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return organization;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var organization = await _dbContext.Organizations.FindAsync([id], cancellationToken);
        if (organization is null)
        {
            return false;
        }

        _dbContext.Organizations.Remove(organization);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
