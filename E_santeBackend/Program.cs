using E_santeBackend.API.Middlewares;
using E_santeBackend.Application.Interfaces;
using E_santeBackend.Application.Services;
using E_santeBackend.Infrastructure.Data;
using E_santeBackend.Infrastructure.Repositories;
using E_santeBackend.Shared.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace E_santeBackend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database - PostgreSQL
            builder.Services.AddDbContext<EHealthDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            builder.Services.AddScoped<PatientRepository>();
            builder.Services.AddScoped<PharmacienRepository>();
            builder.Services.AddScoped<MedecinRepository>();
            builder.Services.AddScoped<OrdonnanceRepository>();
            builder.Services.AddScoped<StockRepository>();
            builder.Services.AddScoped<MedicamentRepository>();
            builder.Services.AddScoped<PaiementRepository>();
            builder.Services.AddScoped<NotificationRepository>();

            // Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IMedecinService, MedecinService>();
            builder.Services.AddScoped<IPharmacienService, PharmacienService>();
            builder.Services.AddScoped<IMedicamentService, MedicamentService>();
            builder.Services.AddScoped<IOrdonnanceService, OrdonnanceService>();
            builder.Services.AddScoped<IStockService, StockService>();
            builder.Services.AddScoped<IPaiementService, PaiementService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            // Helpers
            builder.Services.AddScoped<JwtHelper>();

            // CORS Configuration for Blazor
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazor", policy =>
                {
                    policy.WithOrigins(
                              "https://localhost:7004", // Blazor WASM HTTPS dev server
                              "https://localhost:7002", // legacy/alternate
                              "http://localhost:5002",
                              "http://localhost:5159",
                              "http://127.0.0.1:5159")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // JWT Authentication
            var jwtKey = builder.Configuration["Jwt:Key"];
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Initialize Database with seed data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<EHealthDbContext>();
                    await DbInitializer.InitializeAsync(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Une erreur s'est produite lors de l'initialisation de la base de donn�es.");
                }
            }

            // Middleware
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Redirect to HTTPS only outside Development to avoid ERR_CONNECTION_REFUSED when only HTTP is running locally
            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            // Enable CORS
            app.UseCors("AllowBlazor");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
