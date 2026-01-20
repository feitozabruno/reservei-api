using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Reservei.Api.Entities;

public class User : IdentityUser
{
    [StringLength(50, MinimumLength = 3)] public string? FullName { get; set; }
}
