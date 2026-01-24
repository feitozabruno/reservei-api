using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservei.Api.Data;
using Reservei.Api.DTOs;

namespace Reservei.Tests.Integration.Professionals;

[Collection("Integration")]
public class ProfessionalsControllerTests(AspireIntegrationFactory factory)
    : IntegrationTestBase(factory)
{
    private async Task<string> RegisterUserAsync(string email, string password)
    {
        var payload = new { email, password };
        var response = await Client.PostAsJsonAsync("/register", payload);
        response.EnsureSuccessStatusCode();

        await using var dbContext = await Factory.GetDbContextAsync<AppDbContext>();

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user!.Id;
    }

    private async Task<string> GetAuthTokenAsync(string email, string password)
    {
        var response = await Client.PostAsJsonAsync("/login", new { email, password });
        response.EnsureSuccessStatusCode();

        var loginResult = await response.Content.ReadFromJsonAsync<AccessTokenResponse>();
        return loginResult!.AccessToken;
    }

    private static CreateProfessionalDto CreateValidPayload()
    {
        return new CreateProfessionalDto
        {
            BusinessName = "GitHub",
            Bio = "Escrevo código.",
            Specialty = "Desenvolvedor C#/.NET",
            ProfilePhotoUrl = "https://github.com/feitozabruno.png",
            Timezone = "America/Sao_Paulo",
            AppointmentDurationMinutes = 30,
            Address = "Ipanema, 2125"
        };
    }

    [Fact]
    public async Task Create_Professional_ShouldReturn_CreatedStatus()
    {
        var email = $"test_{Guid.NewGuid()}@email.com";
        const string password = "Password123!";

        var userId = await RegisterUserAsync(email, password);
        var token = await GetAuthTokenAsync(email, password);

        var payload = CreateValidPayload();

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await Client.PostAsJsonAsync("/api/professionals", payload);
        var content = await response.Content.ReadFromJsonAsync<ProfessionalResponseDto>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(content);
        Assert.NotEqual(Guid.Empty, content.Id);
        Assert.Equal(payload.BusinessName, content.BusinessName);
        Assert.Equal(userId, content.UserId);
        Assert.True(content.IsActive);
    }

    [Fact]
    public async Task Create_ShouldReturnConflict_WhenUserAlreadyHasProfile()
    {
        var email = $"test_{Guid.NewGuid()}@email.com";
        const string password = "Password123!";

        await RegisterUserAsync(email, password);
        var token = await GetAuthTokenAsync(email, password);

        var payload = CreateValidPayload();

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var initialResponse = await Client.PostAsJsonAsync("/api/professionals", payload);
        initialResponse.EnsureSuccessStatusCode();

        var duplicateResponse = await Client.PostAsJsonAsync("/api/professionals", payload);
        var errorResponse = await duplicateResponse.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.Equal(HttpStatusCode.Conflict, duplicateResponse.StatusCode);
        Assert.NotNull(errorResponse);

        Assert.Equal("Conflito de Regra de Negócio", errorResponse.Title);
        Assert.Equal("User already has a professional profile", errorResponse.Detail);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenProfessionalExists()
    {
        var email = $"test_{Guid.NewGuid()}@email.com";
        const string password = "Password123!";

        var userId = await RegisterUserAsync(email, password);
        var token = await GetAuthTokenAsync(email, password);
        var payload = CreateValidPayload();

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var createResponse = await Client.PostAsJsonAsync("/api/professionals", payload);
        createResponse.EnsureSuccessStatusCode();

        var createdProfessional = await createResponse.Content.ReadFromJsonAsync<ProfessionalResponseDto>();

        Client.DefaultRequestHeaders.Authorization = null;

        var response = await Client.GetAsync($"/api/professionals/{createdProfessional!.Id}");
        var content = await response.Content.ReadFromJsonAsync<ProfessionalResponseDto>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.Equal(createdProfessional.Id, content.Id);
        Assert.Equal(userId, content.UserId);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        Client.DefaultRequestHeaders.Authorization = null;

        var response = await Client.GetAsync($"/api/professionals/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    internal record AccessTokenResponse(string AccessToken);
}
