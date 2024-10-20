using Infrastructure.Domain.Maintenance;
using Infrastructure.Domain.Rooms;
using Infrastructure.Domain.Tenants;
using Infrastructure.Persistence.Abstractions.Models.Coliving;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;
public class ColivingReservationsDbContext : IdentityDbContext<IdentityUser>
{
    public ColivingReservationsDbContext(DbContextOptions<ColivingReservationsDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Coliving> Colivings { get; set; }
    
    public DbSet<Room> Rooms { get; set; }
    
    public DbSet<Tenant> Tenants { get; set; }
    
    public DbSet<Maintenance> Maintenances { get; set; }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
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
        
        modelBuilder.Entity<Maintenance>()
            .HasOne(r => r.AssignedRoom)
            .WithMany(r => r.Maintenances)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Maintenance>()
            .HasOne(m => m.Tenant)  
            .WithMany(t => t.Maintenances)  
            .HasForeignKey(m => m.TenantId) 
            .OnDelete(DeleteBehavior.SetNull);
    }

}