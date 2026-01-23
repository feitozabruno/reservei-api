using System.Net;
using System.Net.Http.Json;
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

    private static CreateProfessionalDto CreateValidPayload(string userId)
    {
        return new CreateProfessionalDto
        {
            UserId = userId,
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
        var userId = await RegisterUserAsync(email, "Password123!");
        var payload = CreateValidPayload(userId);

        var response = await Client.PostAsJsonAsync("/api/professionals", payload);
        var content = await response.Content.ReadFromJsonAsync<ProfessionalResponseDto>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(content);
        Assert.NotEqual(Guid.Empty, content.Id);
        Assert.Equal(payload.BusinessName, content.BusinessName);
        Assert.Equal(userId, content.UserId);
        Assert.Equal(payload.Bio, content.Bio);
        Assert.Equal(payload.Specialty, content.Specialty);
        Assert.Equal(payload.ProfilePhotoUrl, content.ProfilePhotoUrl);
        Assert.Equal(payload.Timezone, content.Timezone);
        Assert.Equal(payload.AppointmentDurationMinutes, content.AppointmentDurationMinutes);
        Assert.Equal(payload.Address, content.Address);
        Assert.True(content.IsActive);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenUserAlreadyHasProfile()
    {
        var email = $"test_{Guid.NewGuid()}@email.com";
        var userId = await RegisterUserAsync(email, "Password123!");
        var payload = CreateValidPayload(userId);

        var initialResponse = await Client.PostAsJsonAsync("/api/professionals", payload);
        initialResponse.EnsureSuccessStatusCode();

        var duplicateResponse = await Client.PostAsJsonAsync("/api/professionals", payload);
        var errorResponse = await duplicateResponse.Content.ReadFromJsonAsync<ErrorResponse>();

        Assert.Equal(HttpStatusCode.BadRequest, duplicateResponse.StatusCode);
        Assert.NotNull(errorResponse);
        Assert.Equal("User already has a professional profile", errorResponse.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        var nonExistentUserId = Guid.NewGuid().ToString();
        var payload = CreateValidPayload(nonExistentUserId);

        var response = await Client.PostAsJsonAsync("/api/professionals", payload);
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(errorResponse);
        Assert.Equal("User not found", errorResponse.Message);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenProfessionalExists()
    {
        var email = $"test_{Guid.NewGuid()}@email.com";
        var userId = await RegisterUserAsync(email, "Password123!");
        var payload = CreateValidPayload(userId);

        var createResponse = await Client.PostAsJsonAsync("/api/professionals", payload);
        createResponse.EnsureSuccessStatusCode();

        var createdProfessional = await createResponse.Content.ReadFromJsonAsync<ProfessionalResponseDto>();
        Assert.NotNull(createdProfessional);

        var response = await Client.GetAsync($"/api/professionals/{createdProfessional.Id}");
        var content = await response.Content.ReadFromJsonAsync<ProfessionalResponseDto>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.Equal(createdProfessional.Id, content.Id);
        Assert.Equal(userId, content.UserId);
        Assert.Equal(payload.BusinessName, content.BusinessName);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var response = await Client.GetAsync($"/api/professionals/{Guid.NewGuid().ToString()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    internal record ErrorResponse(string Message);
}
