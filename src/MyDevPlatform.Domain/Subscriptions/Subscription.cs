namespace MyDevPlatform.Domain.Subscriptions;

public class Subscription
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrganizationId { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Trial;
    public DateTime? StartsAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public bool IsAvailable()
    {
        if (Status == SubscriptionStatus.Active || Status == SubscriptionStatus.Trial)
        {
            return ExpiresAt is null || ExpiresAt.Value > DateTime.UtcNow;
        }

        return false;
    }
}
