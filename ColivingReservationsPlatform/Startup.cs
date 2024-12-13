using System.Text;
using System.Text.Json.Serialization;
using Application.Abstractions.Colivings;
using Application.Abstractions.Room;
using Application.Abstractions.Tenant;
using Application.Abstractions.User;
using Application.Contracts;
using Application.Contracts.Maintenance;
using Application.Contracts.Room;
using Application.Contracts.Tenant;
using Application.Validators;
using FluentValidation;
using Infrastructure.Domain;
using Infrastructure.Domain.User;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Abstractions;
using Infrastructure.Services.Colivings;
using Infrastructure.Services.Maintenance;
using Infrastructure.Services.Rooms;
using Infrastructure.Services.Tenants;
using Infrastructure.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddDbContext<ColivingReservationsDbContext>(options =>
            options.UseNpgsql(Configuration["ConnectionStrings:PostgreSql"]!));

        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ColivingReservationsDbContext>()
            .AddDefaultTokenProviders();
        
        var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]!);
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
        
        services.AddHttpContextAccessor();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IColivingService, ColivingService>();
        services.AddTransient<IRoomService, RoomService>();
        services.AddTransient<ITenantService, TenantService>();
        services.AddTransient<IMaintenanceService, MaintenanceService>();
        services.AddHostedService<MigrationService>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddTransient<IValidator<ColivingCreateDto>, ColivingRequestValidator>();
        services.AddTransient<IValidator<RoomCreateDto>, RoomValidator.RoomCreateRequestValidator>();
        services.AddTransient<IValidator<TenantCreateDto>, TenantCreateRequestValidator>();
        services.AddTransient<IValidator<AssignTenantDto>, RoomValidator.TenantAssignRequestValidator>();
        services.AddTransient<IValidator<MaintenanceCreateDto>, MaintenanceRequestValidator>();
        
        services.AddSwaggerGen();
        services.AddOptions();
    }

    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/swagger/index.html", async context =>
            {
                context.Response.Redirect("/swagger/index.html");
            }).AllowAnonymous();
            endpoints.MapSwagger();
        });
        
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var services = serviceScope.ServiceProvider;
        }
    }
}
