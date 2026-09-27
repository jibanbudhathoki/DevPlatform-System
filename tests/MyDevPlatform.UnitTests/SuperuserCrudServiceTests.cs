using MyDevPlatform.Application.Organizations;
using MyDevPlatform.Application.Projects;
using MyDevPlatform.Application.Users;
using MyDevPlatform.Contracts.Organizations;
using MyDevPlatform.Domain.Organizations;
using MyDevPlatform.Domain.Projects;
using MyDevPlatform.Domain.Users;

namespace MyDevPlatform.UnitTests;

public class SuperuserCrudServiceTests
{
    [Fact]
    public async Task PlatformProjectIsIndependentFromOrganizations()
    {
        var repository = new PlatformProjectRepositoryStub();
        var service = new PlatformProjectService(repository);

        await service.CreateProjectAsync("Platform Owner Project", "", "Internal tooling", "platform/owner-project", "dotnet", true);

        Assert.Equal("platform-owner-project", repository.AddedProject?.Slug);
        Assert.Null(typeof(PlatformProject).GetProperty(nameof(Project.OrganizationId)));
    }

    [Fact]
    public async Task OrganizationWithTenantProjectsCannotBeDeleted()
    {
        var repository = new OrganizationRepositoryStub { ProjectsExist = true };
        var service = new OrganizationService(repository);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteOrganizationAsync(Guid.NewGuid()));

        Assert.False(repository.DeleteCalled);
    }

    [Fact]
    public async Task UserCreationNormalizesEmailAndValidatesRole()
    {
        var repository = new UserRepositoryStub();
        var service = new UserService(repository);

        await service.CreateUserAsync("Platform User", "USER@EXAMPLE.COM", "Developer", true);

        Assert.Equal("user@example.com", repository.AddedUser?.Email);
        Assert.Equal(UserRole.Developer, repository.AddedUser?.Role);
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateUserAsync("Invalid Role", "invalid@example.com", "Superuser", true));
    }

    private sealed class OrganizationRepositoryStub : IOrganizationRepository
    {
        public bool ProjectsExist { get; init; }
        public bool DeleteCalled { get; private set; }

        public Task<IReadOnlyCollection<OrganizationSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyCollection<OrganizationSummaryDto>>([]);
        public Task<Organization?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult<Organization?>(null);
        public Task<bool> SlugExistsAsync(string slug, Guid? exceptId = null, CancellationToken cancellationToken = default) => Task.FromResult(false);
        public Task<bool> HasProjectsAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(ProjectsExist);
        public Task<Organization> AddAsync(Organization organization, CancellationToken cancellationToken = default) => Task.FromResult(organization);
        public Task<Organization> UpdateAsync(Organization organization, CancellationToken cancellationToken = default) => Task.FromResult(organization);

        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            DeleteCalled = true;
            return Task.FromResult(true);
        }
    }

    private sealed class UserRepositoryStub : IUserRepository
    {
        public User? AddedUser { get; private set; }

        public Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyCollection<User>>([]);
        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult<User?>(AddedUser?.Id == id ? AddedUser : null);
        public Task<bool> EmailExistsAsync(string email, Guid? exceptId = null, CancellationToken cancellationToken = default) => Task.FromResult(false);

        public Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            AddedUser = user;
            return Task.FromResult(user);
        }

        public Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default) => Task.FromResult(user);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(false);
    }

    private sealed class PlatformProjectRepositoryStub : IPlatformProjectRepository
    {
        public PlatformProject? AddedProject { get; private set; }

        public Task<IReadOnlyCollection<PlatformProject>> GetAllAsync(CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyCollection<PlatformProject>>([]);
        public Task<PlatformProject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult<PlatformProject?>(null);
        public Task<bool> SlugExistsAsync(string slug, Guid? exceptId = null, CancellationToken cancellationToken = default) => Task.FromResult(false);

        public Task<PlatformProject> AddAsync(PlatformProject project, CancellationToken cancellationToken = default)
        {
            AddedProject = project;
            return Task.FromResult(project);
        }

        public Task<PlatformProject> UpdateAsync(PlatformProject project, CancellationToken cancellationToken = default) => Task.FromResult(project);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(false);
    }
}