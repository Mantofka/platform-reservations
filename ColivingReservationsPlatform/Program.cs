using System.Text.Json.Serialization;
using Application.Abstractions.Colivings;
using Application.Abstractions.Room;
using Application.Abstractions.Tenant;
using Application.Contracts;
using Application.Contracts.Room;
using Application.Contracts.Tenant;
using Application.Validators;
using FluentValidation;
using Infrastructure.Domain;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Abstractions;
using Infrastructure.Services.Colivings;
using Infrastructure.Services.Rooms;
using Infrastructure.Services.Tenants;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<ColivingReservationsDbContext>(options => options.UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSql"]));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IColivingService, ColivingService>();
builder.Services.AddTransient<IRoomService, RoomService>();
builder.Services.AddTransient<ITenantService, TenantService>();
builder.Services.AddHostedService<MigrationService>();
builder.Services.AddTransient<IValidator<ColivingCreateDto>, ColivingRequestValidator>();
builder.Services.AddTransient<IValidator<RoomCreateDto>, RoomValidator.RoomCreateRequestValidator>();
builder.Services.AddTransient<IValidator<TenantCreateDto>, TenantCreateRequestValidator>();
builder.Services.AddTransient<IValidator<AssignTenantDto>, RoomValidator.TenantAssignRequestValidator>();



builder.Services.AddSwaggerGen();
builder.Services.AddOptions();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
