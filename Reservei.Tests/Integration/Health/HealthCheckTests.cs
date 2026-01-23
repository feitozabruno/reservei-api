using System.Net;

namespace Reservei.Tests.Integration.Health;

[Collection("Integration")]
public class HealthCheckTests(AspireIntegrationFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task Get_Health_ReturnsHealthy_And_OkStatus()
    {
        var response = await Client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Healthy", content);
    }
}
