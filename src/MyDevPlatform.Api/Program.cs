using Microsoft.EntityFrameworkCore;
using MyDevPlatform.Application.Organizations;
using MyDevPlatform.Infrastructure.DependencyInjection;
using MyDevPlatform.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddScoped<OrganizationService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Port=5432;Database=mydevplatform;Username=postgres;Password=postgres";

builder.Services.AddDbContext<MyDevPlatformDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.UseCors("DefaultCorsPolicy");
app.UseSerilogRequestLogging();

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/live");
app.MapHealthChecks("/health/ready");
app.MapGet("/api/health", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow }));
app.MapControllers();

app.Run();
