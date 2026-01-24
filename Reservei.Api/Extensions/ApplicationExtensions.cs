using Reservei.Api.Services;

namespace Reservei.Api.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(config => config.AddMaps(typeof(Program).Assembly));

        // Injection of Dependence on Your Services
        services.AddScoped<IProfessionalService, ProfessionalService>();

        return services;
    }
}
