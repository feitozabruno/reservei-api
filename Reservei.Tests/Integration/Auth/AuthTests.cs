using System.Net.Http.Json;

namespace Reservei.Tests.Integration.Auth;

public class AuthTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Register_And_Login_ShouldReturn_AccessToken()
    {
        var uniqueEmail = $"test_{Guid.NewGuid()}@email.com";
        const string
            password = "Password123!"; // Precisa ter Maiúscula, minúscula e caractere especial (padrão do Identity)

        var registerRequest = new { email = uniqueEmail, password };
        var loginRequest = new { email = uniqueEmail, password };

        var registerResponse = await _client.PostAsJsonAsync("/register", registerRequest);

        registerResponse.EnsureSuccessStatusCode(); // Garante que deu 200 OK

        var loginResponse = await _client.PostAsJsonAsync("/login", loginRequest);

        loginResponse.EnsureSuccessStatusCode();

        var loginContent = await loginResponse.Content.ReadFromJsonAsync<LoginResult>();

        Assert.NotNull(loginContent);
        Assert.False(string.IsNullOrEmpty(loginContent.AccessToken)); // O Token chegou!
    }
}

internal record LoginResult(string AccessToken, string RefreshToken, int ExpiresIn);
