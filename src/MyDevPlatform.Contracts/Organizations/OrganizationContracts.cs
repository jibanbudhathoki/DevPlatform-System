namespace MyDevPlatform.Contracts.Organizations;

public record CreateOrganizationRequest(
    string Name,
    string Email,
    string Slug,
    string OwnerName,
    string OwnerEmail);

public record OrganizationSummaryDto(
    Guid Id,
    string Name,
    string Slug,
    string Email,
    string Status,
    string? SubscriptionStatus,
    DateTime CreatedAt);

public record OrganizationDetailsDto(
    Guid Id,
    string Name,
    string Slug,
    string Email,
    string Status,
    string SubscriptionStatus,
    DateTime CreatedAt,
    string? BaseApiUrl);
