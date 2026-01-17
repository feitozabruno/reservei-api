using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Reservei.Api.Entities;

public class User : IdentityUser
{
    [StringLength(50, MinimumLength = 3)]
    public string? FullName { get; set; }
}
