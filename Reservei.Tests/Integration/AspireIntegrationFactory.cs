using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Projects;
using Reservei.Api.Data;
using Respawn;

namespace Reservei.Tests.Integration;

public class AspireIntegrationFactory : IAsyncLifetime
{
    private DistributedApplication _app = null!;
    private NpgsqlConnection _dbConnection = null!;
    private Respawner _respawner = null!;

    public HttpClient Client { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        // 1. Inicia o AppHost
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Reservei_AppHost>();
        appHost.Configuration["ASPIRE_DISABLE_DASHBOARD"] = "true";

        // appHost.Environment.EnvironmentName = "Testing";
        _app = await appHost.BuildAsync();
        await _app.StartAsync();

        Client = _app.CreateHttpClient("api");
        var connectionString = await _app.GetConnectionStringAsync("sqldb");

        // --- Passo de Migração ---
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        await using var dbContext = new AppDbContext(optionsBuilder.Options);
        await dbContext.Database.MigrateAsync();

        // --- Configuração do Respawn ---
        _dbConnection = new NpgsqlConnection(connectionString);
        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
            TablesToIgnore = ["__EFMigrationsHistory"]
        });
    }

    public async Task DisposeAsync()
    {
        await _dbConnection.DisposeAsync();
        await _app.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task<TContext> GetDbContextAsync<TContext>() where TContext : DbContext
    {
        var optionsBuilder = new DbContextOptionsBuilder<TContext>();
        optionsBuilder.UseNpgsql(_dbConnection);
        return (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options)!;
    }
}
