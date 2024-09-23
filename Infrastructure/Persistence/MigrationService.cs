using Infrastructure.Persistence.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public class MigrationService(IServiceScopeFactory scopeFactory) : BaseMigrationService<ColivingReservationsDbContext>(scopeFactory)
{
}