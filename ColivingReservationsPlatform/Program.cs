using System.Text.Json.Serialization;
using Application.Abstractions.Coliving;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Abstractions;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
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
builder.Services.AddHostedService<MigrationService>();

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
