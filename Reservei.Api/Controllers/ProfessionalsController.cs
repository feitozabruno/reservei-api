using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservei.Api.Data;
using Reservei.Api.DTOs;
using Reservei.Api.Entities;

namespace Reservei.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfessionalsController(AppDbContext context) : ControllerBase
{
    private static ProfessionalResponseDto MapToResponseDto(Professional professional)
    {
        return new ProfessionalResponseDto
        {
            Id = professional.Id,
            UserId = professional.UserId,
            BusinessName = professional.BusinessName,
            Bio = professional.Bio,
            Specialty = professional.Specialty,
            ProfilePhotoUrl = professional.ProfilePhotoUrl,
            CoverPictureUrl = professional.CoverPictureUrl,
            Timezone = professional.Timezone,
            AppointmentDurationMinutes = professional.AppointmentDurationMinutes,
            Address = professional.Address,
            IsActive = professional.IsActive,
            CreatedAt = professional.CreatedAt,
            UpdatedAt = professional.UpdatedAt,
            FullName = professional.User.FullName,
            Email = professional.User.Email
        };
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProfessionalResponseDto>> GetById([FromRoute] Guid id)
    {
        var professional = await context.Professionals.Include(professional => professional.User)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (professional == null)
            return NotFound();

        return Ok(MapToResponseDto(professional));
    }

    [HttpPost]
    public async Task<ActionResult<ProfessionalResponseDto>> Create([FromBody] CreateProfessionalDto dto)
    {
        var user = await context.Users.FindAsync(dto.UserId);
        if (user == null)
            return NotFound(new { message = "User not found" });

        var exists = await context.Professionals.AnyAsync(p => p.UserId == dto.UserId);
        if (exists)
            return BadRequest(new { message = "User already has a professional profile" });

        var professional = new Professional
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            BusinessName = dto.BusinessName,
            Bio = dto.Bio,
            Specialty = dto.Specialty,
            ProfilePhotoUrl = dto.ProfilePhotoUrl,
            CoverPictureUrl = dto.CoverPictureUrl,
            Timezone = dto.Timezone,
            AppointmentDurationMinutes = dto.AppointmentDurationMinutes,
            Address = dto.Address,
            CreatedAt = DateTime.UtcNow
        };

        await context.Professionals.AddAsync(professional);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = professional.Id }, MapToResponseDto(professional));
    }
}
