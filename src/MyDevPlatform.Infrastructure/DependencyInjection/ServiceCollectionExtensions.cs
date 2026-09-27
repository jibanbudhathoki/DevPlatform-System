using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyDevPlatform.Application.Organizations;
using MyDevPlatform.Infrastructure.Persistence;

namespace MyDevPlatform.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<MyDevPlatformDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        return services;
    }
}
