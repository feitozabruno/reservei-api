using Reservei.Api.DTOs;

namespace Reservei.Api.Services.Professionals;

public interface IProfessionalService
{
    Task<ProfessionalResponseDto?> GetByIdAsync(Guid id);
    Task<ProfessionalResponseDto> CreateAsync(string userId, CreateProfessionalDto dto);
}
