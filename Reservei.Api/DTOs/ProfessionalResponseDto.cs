namespace Reservei.Api.DTOs;

public class ProfessionalResponseDto
{
    public required Guid Id { get; init; }
    public required string UserId { get; init; }
    public required string BusinessName { get; init; }
    public string? Bio { get; init; }
    public required string Specialty { get; init; }
    public string? ProfilePhotoUrl { get; init; }
    public string? CoverPictureUrl { get; init; }
    public required string Timezone { get; init; }
    public required int AppointmentDurationMinutes { get; init; }
    public string? Address { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public required bool IsActive { get; init; }

    public string? FullName { get; init; }
    public string? Email { get; init; }
}
