namespace Reservei.Api.Entities;
using System.ComponentModel.DataAnnotations;

public class Professional
{
    public Guid Id { get; set; }
    public required string UserId { get; set; }
    public User User { get; set; } = null!;

    [StringLength(100)]
    public required string BusinessName  { get; set; }

    [StringLength(500)]
    public string? Bio  { get; set; }

    [StringLength(50)]
    public required string Specialty  { get; set; }

    [StringLength(500)]
    public string? ProfilePhotoUrl  { get; set; }

    [StringLength(500)]
    public string? CoverPictureUrl  { get; set; }

    [StringLength(50)]
    public required string Timezone  { get; set; }

    [Range(15, 480)]
    public required int AppointmentDurationMinutes { get; set; }

    [StringLength(300)]
    public string? Address { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
