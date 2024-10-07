using Application.Contracts;
using Infrastructure.Domain.Rooms;
using Infrastructure.Domain.Tenants;
using Infrastructure.Persistence.Abstractions.Models;
using Infrastructure.Persistence.Abstractions.Models.Coliving;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;
public class ColivingReservationsDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Coliving> Colivings { get; set; }
    
    public DbSet<Room> Rooms { get; set; }
    
    public DbSet<Tenant> Tenants { get; set; }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>()
            .HasOne(r => r.Coliving)
            .WithMany(r => r.Rooms)
            .HasForeignKey(r => r.ColivingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Room>()
            .HasMany(r => r.Tenants)
            .WithMany(t => t.Rooms)
            .UsingEntity<Dictionary<string, object>>(
                "RoomTenant",
                room => room.HasOne<Tenant>().WithMany().HasForeignKey("TenantId"),
                tenant => tenant.HasOne<Room>().WithMany().HasForeignKey("RoomId"),
                join =>
                {
                    join.HasKey("RoomId", "TenantId");
                });
    }

}