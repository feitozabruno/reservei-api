using System.ComponentModel.DataAnnotations;

namespace Reservei.Api.DTOs;

public class CreateProfessionalDto
{
    [StringLength(100, MinimumLength = 3)] public required string BusinessName { get; set; }
    [StringLength(500)] public string? Bio { get; set; }
    [StringLength(50, MinimumLength = 3)] public required string Specialty { get; set; }
    [StringLength(500)] public string? ProfilePhotoUrl { get; set; }
    [StringLength(500)] public string? CoverPictureUrl { get; set; }
    [StringLength(50)] public required string Timezone { get; set; }
    [Range(15, 480)] public required int AppointmentDurationMinutes { get; set; }
    [StringLength(300)] public string? Address { get; set; }
}
