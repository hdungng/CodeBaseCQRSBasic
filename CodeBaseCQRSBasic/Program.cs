using CodeBaseCQRSBasic.Infrastructure;
using CodeBaseCQRSBasic.Seed;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        await app.RunAsync();
    }
}
