using Application.Contracts;
using Infrastructure.Persistence.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;
public class ColivingReservationsDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Coliving> Colivings { get; set; }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

}