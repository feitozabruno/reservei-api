using Microsoft.EntityFrameworkCore;

namespace Reservei.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options);
