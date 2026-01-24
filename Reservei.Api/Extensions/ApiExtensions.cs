using Reservei.Api.Entities;
using Reservei.Api.Middlewares;
using Scalar.AspNetCore;

namespace Reservei.Api.Extensions;

public static class ApiExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApiDocumentation()
        {
            services.AddOpenApi();
            return services;
        }

        public IServiceCollection AddApiErrorHandling()
        {
            services.AddProblemDetails();
            services.AddExceptionHandler<GlobalExceptionHandler>();

            return services;
        }
    }

    extension(WebApplication app)
    {
        public WebApplication UseApiDocumentation()
        {
            if (!app.Environment.IsDevelopment())
                return app;

            app.MapOpenApi();
            app.MapScalarApiReference();

            return app;
        }

        public WebApplication MapProjectEndpoints()
        {
            app.MapIdentityApi<User>().WithTags("Autenticação");
            app.MapControllers();
            app.MapGet("/", () => "Reservei API is running!");

            return app;
        }

        public WebApplication UseApiErrorHandling()
        {
            app.UseExceptionHandler();

            return app;
        }
    }
}
