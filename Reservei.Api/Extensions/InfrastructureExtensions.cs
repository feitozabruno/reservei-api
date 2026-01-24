using Reservei.Api.Data;
using Reservei.Api.Entities;

namespace Reservei.Api.Extensions;

public static class InfrastructureExtensions
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        // Database with Aspire
        builder.AddNpgsqlDbContext<AppDbContext>("sqldb");

        // Autenticação and Identity
        builder.Services.AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<AppDbContext>();

        builder.Services.AddAuthorization();

        return builder;
    }
}
