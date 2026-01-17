using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reservei.Api.Entities;

namespace Reservei.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Professional> Professionals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Professional>(entity =>
        {
            entity
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<Professional>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(p => p.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");
        });
    }
}
