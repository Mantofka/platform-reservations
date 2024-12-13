using Infrastructure.Domain.Maintenance;
using Infrastructure.Domain.Rooms;
using Infrastructure.Domain.RoomTenant;
using Infrastructure.Domain.Tenants;
using Infrastructure.Domain.User;
using Infrastructure.Persistence.Abstractions.Models.Coliving;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;
public class ColivingReservationsDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ColivingReservationsDbContext(DbContextOptions<ColivingReservationsDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Coliving> Colivings { get; set; }
    
    public DbSet<Room> Rooms { get; set; }
    
    public DbSet<Tenant> Tenants { get; set; }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Maintenance> Maintenances { get; set; }
    
    public DbSet<RoomTenant> RoomTenants { get; set; }
    
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
        
        modelBuilder.Entity<RoomTenant>()
            .HasKey(rt => new { rt.RoomId, rt.TenantId });
        
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