namespace Reservei.Tests.Integration;

public abstract class IntegrationTestBase(CustomWebApplicationFactory factory) : IAsyncLifetime
{
    protected readonly HttpClient Client = factory.CreateClient();
    protected readonly CustomWebApplicationFactory Factory = factory;

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await Factory.ResetDatabaseAsync();
    }
}
