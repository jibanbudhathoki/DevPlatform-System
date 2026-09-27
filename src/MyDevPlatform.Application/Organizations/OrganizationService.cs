using MyDevPlatform.Contracts.Organizations;
using MyDevPlatform.Domain.Organizations;
using MyDevPlatform.Domain.Subscriptions;

namespace MyDevPlatform.Application.Organizations;

public class OrganizationService
{
    private readonly IOrganizationRepository _repository;

    public OrganizationService(IOrganizationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<OrganizationSummaryDto>> GetOrganizationsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }

    public async Task<OrganizationDetailsDto> CreateOrganizationAsync(CreateOrganizationRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Organization name is required.", nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains('@'))
        {
            throw new ArgumentException("A valid organization email is required.", nameof(request));
        }

        var organization = new Organization
        {
            Name = request.Name.Trim(),
            Slug = string.IsNullOrWhiteSpace(request.Slug) ? CreateSlug(request.Name) : CreateSlug(request.Slug),
            Email = request.Email.Trim(),
            Status = OrganizationStatus.Active,
            Subscription = new Subscription
            {
                Status = SubscriptionStatus.Trial,
                StartsAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            }
        };

        var created = await _repository.AddAsync(organization, cancellationToken);

        return new OrganizationDetailsDto(
            created.Id,
            created.Name,
            created.Email,
            created.Status.ToString(),
            created.Subscription?.Status.ToString() ?? SubscriptionStatus.Trial.ToString(),
            created.CreatedAt,
            created.BaseApiUrl);
    }

    private static string CreateSlug(string value)
    {
        var normalized = value.Trim();
        var slug = new string(normalized.Where(ch => char.IsLetterOrDigit(ch) || ch == '-' || ch == '_').ToArray());
        return string.IsNullOrWhiteSpace(slug) ? "organization" : slug.ToLowerInvariant();
    }
}
