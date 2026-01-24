using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservei.Api.DTOs;
using Reservei.Api.Services;

namespace Reservei.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfessionalsController(IProfessionalService professionalService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProfessionalResponseDto>> GetById([FromRoute] Guid id)
    {
        var result = await professionalService.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProfessionalResponseDto>> Create([FromBody] CreateProfessionalDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await professionalService.CreateAsync(userId, dto);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
