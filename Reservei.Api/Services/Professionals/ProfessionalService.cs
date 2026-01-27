using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservei.Api.Data;
using Reservei.Api.DTOs;
using Reservei.Api.Entities;

namespace Reservei.Api.Services.Professionals;

public class ProfessionalService(AppDbContext context, IMapper mapper) : IProfessionalService
{
    public async Task<ProfessionalResponseDto?> GetByIdAsync(Guid id)
    {
        var professional = await context.Professionals
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);

        return professional == null ? null : mapper.Map<ProfessionalResponseDto>(professional);
    }

    public async Task<ProfessionalResponseDto> CreateAsync(string userId, CreateProfessionalDto dto)
    {
        var user = await context.Users.FindAsync(userId);
        if (user == null)
            throw new ArgumentException("User not found");

        var exists = await context.Professionals.AnyAsync(p => p.UserId == userId);
        if (exists)
            throw new InvalidOperationException("User already has a professional profile");


        var professional = mapper.Map<Professional>(dto);

        professional.Id = Guid.NewGuid();
        professional.UserId = userId;
        professional.CreatedAt = DateTime.UtcNow;

        await context.Professionals.AddAsync(professional);
        await context.SaveChangesAsync();

        professional.User = user;

        return mapper.Map<ProfessionalResponseDto>(professional);
    }
}
