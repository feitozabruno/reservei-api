namespace Reservei.Tests.Integration;

public abstract class IntegrationTestBase(AspireIntegrationFactory factory) : IAsyncLifetime
{
    protected readonly HttpClient Client = factory.Client;
    protected readonly AspireIntegrationFactory Factory = factory;

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await Factory.ResetDatabaseAsync();
    }
}
