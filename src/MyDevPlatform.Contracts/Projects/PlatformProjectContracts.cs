namespace MyDevPlatform.Contracts.Projects;

public record PlatformProjectSummaryDto(
    Guid Id,
    string Name,
    string Slug,
    string Description,
    string RootPath,
    string TemplateId,
    bool IsActive,
    DateTime CreatedAt);