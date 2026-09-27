using System.ComponentModel.DataAnnotations;
using MyDevPlatform.Domain.Projects;
using MyDevPlatform.Domain.Users;

namespace MyDevPlatform.Api.Features.Superuser;

public class OrganizationFormViewModel
{
    public Guid? Id { get; set; }

    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(120)]
    [RegularExpression("^[a-zA-Z0-9_-]+$", ErrorMessage = "Use letters, numbers, hyphens, or underscores.")]
    public string Slug { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = "Active";
}

public class UserFormViewModel
{
    public Guid? Id { get; set; }

    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = nameof(UserRole.Viewer);

    public bool IsActive { get; set; } = true;
}

public class PlatformProjectFormViewModel
{
    public Guid? Id { get; set; }

    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(120)]
    [RegularExpression("^[a-zA-Z0-9_-]+$", ErrorMessage = "Use letters, numbers, hyphens, or underscores.")]
    public string Slug { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required, StringLength(500)]
    public string RootPath { get; set; } = string.Empty;

    [Required, StringLength(150)]
    public string TemplateId { get; set; } = "default";

    public bool IsActive { get; set; } = true;
}

public class UserIndexViewModel
{
    public IReadOnlyCollection<User> Users { get; init; } = [];
    public string? Query { get; init; }
    public bool IsDataAvailable { get; init; } = true;
    public string? ErrorMessage { get; init; }
}

public class PlatformProjectIndexViewModel
{
    public IReadOnlyCollection<PlatformProject> Projects { get; init; } = [];
    public string? Query { get; init; }
    public bool IsDataAvailable { get; init; } = true;
    public string? ErrorMessage { get; init; }
}

public class SuperuserLoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }
    public string? ReturnUrl { get; set; }
}