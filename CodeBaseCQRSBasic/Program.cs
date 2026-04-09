using System.Text;
using CodeBaseCQRSBasic.Infrastructure;
using CodeBaseCQRSBasic.Middleware;
using CodeBaseCQRSBasic.Seed;
using CodeBaseCQRSBasic.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CodeBaseCQRSBasic;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var provider = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "SqlServer";

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            if (provider.Equals("MySql", StringComparison.OrdinalIgnoreCase))
            {
                var mySqlConnection = builder.Configuration.GetConnectionString("MySqlConnection")
                    ?? throw new InvalidOperationException("MySql connection string is not configured.");
                options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection));
            }
            else
            {
                var sqlServerConnection = builder.Configuration.GetConnectionString("SqlServerConnection")
                    ?? throw new InvalidOperationException("SQL Server connection string is not configured.");
                options.UseSqlServer(sqlServerConnection);
            }
        });

        var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddSingleton<IPasswordHasherService, PasswordHasherService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await DatabaseSeeder.SeedAsync(context);
        }

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        await app.RunAsync();
    }
}
