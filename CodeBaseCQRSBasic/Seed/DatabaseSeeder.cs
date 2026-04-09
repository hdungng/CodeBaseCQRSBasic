using CodeBaseCQRSBasic.Domain;
using CodeBaseCQRSBasic.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodeBaseCQRSBasic.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Roles.AnyAsync() || await context.Users.AnyAsync())
        {
            return;
        }

        var adminRole = new Role { Name = "Admin" };
        var userRole = new Role { Name = "User" };

        context.Roles.AddRange(adminRole, userRole);
        await context.SaveChangesAsync();

        var users = Enumerable.Range(1, 10)
            .Select(index => new User
            {
                UserName = $"user{index}",
                Email = $"user{index}@example.com",
                RoleId = index <= 3 ? adminRole.Id : userRole.Id
            })
            .ToList();

        context.Users.AddRange(users);
        await context.SaveChangesAsync();
    }
}
